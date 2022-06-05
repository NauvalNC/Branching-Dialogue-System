using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace DialogueSystem_2301906331
{
    public class StoryArchiveDriver : MonoBehaviour, IStoryDriver
    {
        [HideInInspector] public StoryArchivePackage archive;
        [HideInInspector] public ArchivePackage rootPackage;

        [Header("Identifications")]
        [SerializeField] private Color normalPackageColor;
        [SerializeField] private Color activePackageColor;
        [SerializeField] private Sprite[] statusIcons;

        [Header("User Interface")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private TMP_Text archiveTitleText;
        [SerializeField] private ScrollRect archiveScrollRect;
        [SerializeField] private GameObject archiveItemPanel;
        [SerializeField] private GameObject memoryItemPanel;
        [SerializeField] private Transform archiveItemCont;
        [SerializeField] private ContentFitToChildFilter archiveContFilter;
        [SerializeField] private GameObject lineDot;
        [SerializeField] private Button returnBtn;
        [SerializeField] private Button quitBtn;

        [Header("Settings")]
        [SerializeField] private Vector2 padding;
        [SerializeField] private Vector2 itemPanelGap;
        [SerializeField] private float startLineOffset = 10f;
        [SerializeField] private float minZoom, maxZoom;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private bool invertMobileZoom = false;

        private Animator canvasAC;
        private Vector2 chapterPanelSize;
        private List<GameObject> panelPool = new List<GameObject>();
        private bool isFirstLaunch = true;

        void Start()
        {
            canvasAC = canvas.GetComponent<Animator>();
            returnBtn.onClick.AddListener(delegate { StartCoroutine(ReturnToStory()); });
            quitBtn.onClick.AddListener(delegate { Application.Quit(); });
        }

        private void Update()
        {
            returnBtn.gameObject.SetActive(StoryManager.Instance.IsStoryActive());
            ListenZoomInput();
        }

        private void SetupArchiveDriver()
        {
            if (isFirstLaunch)
            {
                isFirstLaunch = false;
                canvasAC.Play("notice_in", -1, 0f);
            } else
            {
                canvasAC.Play("fade_in", -1, 0f);
            }

            ClearStoryTree();

            archiveTitleText.text = archive.GetArchiveTitle();
            chapterPanelSize = archiveItemPanel.GetComponent<RectTransform>().rect.size;

            DrawStoryTree(rootPackage, Vector3.zero);

            ResizeContainer();
        }

        private void ResizeContainer()
        {
            archiveContFilter.RefitSizeToContent();

            Vector2 filterSize = archiveContFilter.GetComponent<RectTransform>().sizeDelta;
            archiveItemCont.GetComponent<RectTransform>().sizeDelta = filterSize + padding * 2f;
            archiveContFilter.transform.localPosition -= new Vector3(filterSize.x / 2f, - (filterSize.y / 2), 0f);

            Vector2 contSize = archiveItemCont.GetComponent<RectTransform>().sizeDelta;
            archiveItemCont.localPosition += new Vector3(contSize.x / 2f, -contSize.y / 2f, 0f);
        }

        /// <summary>
        /// Draw story tree and return the generated panel.
        /// </summary>
        /// <param name="currPackage"></param>
        /// <param name="currPos"></param>
        /// <returns></returns>
        GameObject DrawStoryTree(ArchivePackage currPackage, Vector3 currPos)
        {
            // Get setory package
            StoryPackage story = archive.FindPackageByAddressKey(currPackage.packAddressKey);


            // Instantiate panel item based on story type
            GameObject panel;

            if (story is MemoryStory)
            {
                panel = Instantiate(memoryItemPanel, archiveContFilter.transform);
            } 
            else
            {
                panel = Instantiate(archiveItemPanel, archiveContFilter.transform);
            }
            panel.transform.localPosition = currPos + new Vector3(chapterPanelSize.x / 2, -chapterPanelSize.y / 2);
            panel.transform.localScale = Vector3.one;
            panelPool.Add(panel);


            // Set panel data based on story package type
            Transform temp = panel.transform.Find("panel_cont");

            // If the type is memory story
            if (story is MemoryStory)
            {
                Image bg_temp = panel.transform.Find("background").GetComponent<Image>();
                if (currPackage.status == PackageStatus.DISCOVERED)
                {
                    bg_temp.color = temp.GetComponent<Button>().colors.disabledColor;
                    temp.gameObject.SetActive(false);
                } else
                {
                    bg_temp.color = Color.white;
                    temp.GetComponent<Image>().sprite = ((MemoryStory)story).GetMemory();
                    temp.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(IOpenStoryPackage(currPackage)); });
                }
            } 
            // If the story package is regular story package
            else
            {
                Image status = temp.GetChild(1).GetComponent<Image>();
                if (currPackage.isCurrentPackage) status.sprite = statusIcons[0];
                else status.sprite = statusIcons[(int)currPackage.status];

                if (currPackage.status == PackageStatus.DISCOVERED)
                {
                    temp.GetChild(0).GetComponent<TMP_Text>().text = "???";
                    temp.GetComponent<Button>().interactable = false;
                }
                else
                {
                    temp.GetChild(0).GetComponent<TMP_Text>().text = story.GetStoryTitle();
                    temp.GetComponent<Button>().interactable = true;
                    temp.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(IOpenStoryPackage(currPackage)); });
                }
            }


            // Set panel color based on its status
            if (currPackage.isCurrentPackage) temp.GetComponent<Image>().color = activePackageColor;
            else temp.GetComponent<Image>().color = normalPackageColor;


            // Create horizontal indentation
            currPos.x += itemPanelGap.x;


            // Get line container for drawing lines
            Transform lineCont = panel.transform.Find("line_cont");


            // If has branches, draw branch and the line tree
            int len = currPackage.branches.Count;
            if (len > 0)
            {
                ArchivePackage nextPackage;
                bool canBranch = false;

                GameObject nextPanel = null, line = null;
                Vector2 currPanelPos = Vector2.zero, nextPanelPos = Vector2.zero;
                float verSize = 0, horSize = 0;
                int nextLen = 0;

                // Draw branches
                for (int i = 0; i < len; i++)
                {
                    nextPackage = currPackage.branches[i];
                    if (nextPackage.status == PackageStatus.NOT_OBTAINED) continue;

                    nextLen = nextPackage.branches.Count;

                    // Generate next panel tree (recursively)
                    nextPanel = DrawStoryTree(nextPackage, currPos);

                    currPanelPos = panel.transform.localPosition;
                    nextPanelPos = nextPanel.transform.localPosition;
                    verSize = horSize = 0f;

                    // Draw vertical line
                    if (currPanelPos.y != nextPanelPos.y)
                    {
                        line = Instantiate(lineDot, lineCont);
                        verSize = Mathf.Abs(nextPanelPos.y - currPanelPos.y);
                        line.GetComponent<RectTransform>().sizeDelta += Vector2.up * verSize;
                        line.transform.localPosition += Vector3.right * startLineOffset + Vector3.down * (verSize / 2);
                    }

                    // Draw horizontal line
                    line = Instantiate(lineDot, lineCont);
                    horSize = Mathf.Abs(nextPanelPos.x - currPanelPos.x) - startLineOffset;
                    line.GetComponent<RectTransform>().sizeDelta += new Vector2(horSize, 0f);
                    line.transform.localPosition += Vector3.right * startLineOffset + Vector3.right * (horSize / 2) + Vector3.down * verSize;

                    // Calculate next vertical gap between panels
                    currPos.y += Vector2.down.y * itemPanelGap.y * (nextLen <= 1 ? 1f : nextLen);

                    // If there is branch, then set the panel status = can branch.
                    canBranch = true;
                }

                // If can branch, draw initial branch line
                if (canBranch)
                {
                    line = Instantiate(lineDot, lineCont);
                    line.GetComponent<RectTransform>().sizeDelta += new Vector2(startLineOffset, 0f);
                    line.transform.localPosition += Vector3.right * (startLineOffset / 2);
                }
            }


            return panel;
        }

        private void ListenZoomInput()
        {
            // Pinch
            if (Input.touchCount == 2)
            {
                archiveScrollRect.enabled = false;

                Touch f_Touch = Input.GetTouch(0);
                Touch s_Touch = Input.GetTouch(1);

                Vector2 f_TouchPrevPos = f_Touch.position - f_Touch.deltaPosition;
                Vector2 s_TouchPrevPos = s_Touch.position - s_Touch.deltaPosition;

                float prevMagnitude = (f_TouchPrevPos - s_TouchPrevPos).magnitude;
                float currMagnitude = (f_Touch.position - s_Touch.position).magnitude;

                float diff = currMagnitude - prevMagnitude;

                Zoom(diff * 0.01f * (invertMobileZoom ? -1f : 1f));
            } else
            {
                archiveScrollRect.enabled = true;
            }

            // Mouse scroll
            if (Input.mouseScrollDelta.y != 0)
            {
                Zoom(-Input.mouseScrollDelta.y);
            }
        }

        private void Zoom(float increment)
        {
            float targetZoom = archiveItemCont.localScale.x - increment * zoomSpeed;
            float actualZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            archiveItemCont.localScale = Vector3.one * actualZoom;
        }

        /// <summary>
        /// Open story based on story package address key
        /// </summary>
        /// <param name="addressKey"></param>
        private IEnumerator IOpenStoryPackage(ArchivePackage package)
        {
            if (archive.FindPackageByAddressKey(package.packAddressKey) is MemoryStory == false)
            {
                canvasAC.Play("fade_out", -1, 0f);
                yield return new WaitForSeconds(0.5f);
                StopDriver();
            }
            StoryManager.Instance.LoadStory(package);
        }

        void ClearStoryTree()
        {
            int len = panelPool.Count;
            for (int i = 0; i < len; i++) Destroy(panelPool[i]);
            panelPool.Clear();
        }

        IEnumerator ReturnToStory()
        {
            canvasAC.Play("fade_out", -1, 0f);
            yield return new WaitForSeconds(0.5f);
            StopDriver();
            StoryManager.Instance.ReturnToStoryDialogue();
        }

        public void StartDriver()
        {
            StoryManager.Instance.SaveStoryProgress();
            canvas.SetActive(true);
            SetupArchiveDriver();
        }

        public void StopDriver()
        {
            canvas.SetActive(false);
        }

        public void ResumeDriver() { }

        public IEnumerator IFadeOutAnim(object param)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator IFadeInAnim(object param)
        {
            throw new System.NotImplementedException();
        }
    
        // Save Story Progress
        public void SaveStoryProgress(string filePath)
        {
            filePath = Application.dataPath + "/" + filePath + ".json";
            string jsonString = JsonUtility.ToJson(rootPackage, true);
            System.IO.File.WriteAllText(filePath, jsonString);

            Debug.Log("Story Saved: " + filePath);
        }

        public ArchivePackage LoadStoryProgress(string filePath)
        {
            filePath = Application.dataPath + "/" + filePath + ".json";
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                Debug.Log("Story loaded from " + filePath);
                return JsonUtility.FromJson<ArchivePackage>(json);
            } else
            {
                Debug.Log("No story save file available, start new story instead.");
                return null;
            }
        }
    }
}