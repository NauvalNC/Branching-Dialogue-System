using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using DialogueSystem_2301906331;

namespace DialogueSystem_2301906331
{
    public class DialogueDriver : MonoBehaviour, IStoryDriver
    {
        [HideInInspector] public StoryArchivePackage archive;
        [HideInInspector] public ArchivePackage package;

        [Header("User Interface")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private TMP_Text speaker;
        [SerializeField] private TMP_Text message;
        [SerializeField] private Transform[] branchButtons;
        [SerializeField] private Button toggleSpeedBtn;
        [SerializeField] private Button autoModeBtn;
        [SerializeField] private Button menuBtn;
        [SerializeField] private Image backgroundImg;

        [Header("Controls")]
        [SerializeField] private Color charDisabledColor;
        [SerializeField] private float charImageHeight = 600f;
        [SerializeField] private float printingSpeed = 0.01f;
        [SerializeField] private float readBreakTime = 1f;

        [Header("Avatar Pool")]
        [SerializeField] private int maximumDisplayedAvatar = 2;
        [SerializeField] private Transform avatarPoolCont;
        [SerializeField] private Transform avatarDisplayCont;
        private Dictionary<string, CharacterAvatar> avatarPoolDict = new Dictionary<string, CharacterAvatar>();

        private const string DOUBLE_SPEED = "DOUBLE_SPEED";
        private const string AUTO_MODE = "AUTO_MODE";

        private Animator canvasAC;
        private int chosenBranchIndex = 0;

        private StoryPackage storyPackage;
        private StoryUnpacker story;
        private Dialogue currDialogue;
        private bool allowExecNext = true;

        public bool isAutoMode = false;
        private bool isDoubleSpeed = false;
        private bool isPlaying = false;
        private bool interuptAuto = false;

        private void Start()
        {
            canvasAC = canvas.GetComponent<Animator>();

            // Toggler
            toggleSpeedBtn.onClick.AddListener(delegate { ToggleDoubleSpeed(); });
            autoModeBtn.onClick.AddListener(delegate { ToggleAutoMode(); });
            menuBtn.onClick.AddListener(delegate { StartCoroutine(IOpenMenu()); });
        }

        void SetupDriver()
        {
            // Reset attributtes
            UpdatePlayerPrefs();

            StopAllCoroutines();
            chosenBranchIndex = 0;
            allowExecNext = true;
            ClearAvatarPool();

            // Set package status as visisted, mean the player has already visited this story but didn't complete it yet.
            if (package.status == PackageStatus.DISCOVERED) 
                package.status = PackageStatus.VISITED;

            // Unpack story
            storyPackage = archive.FindPackageByAddressKey(package.packAddressKey);
            storyPackage.Refresh();
            story = new StoryUnpacker(storyPackage.GetJSONFile());

            // Setup branch option interfaces
            backgroundImg.sprite = storyPackage.GetBGImage();
            ClearBranchOptions();
            RepaintUI();

            TryReadCurrentDialogue();
        }

        void RepaintUI()
        {
            if (toggleSpeedBtn != null)
            {
                toggleSpeedBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = isDoubleSpeed ? "2X" : "1X";
            }

            if (autoModeBtn != null)
            {
                autoModeBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = isAutoMode ? "Auto (On)" : "Auto (Off)";
            }
        }

        /// <summary>
        /// Try read next current dialogue
        /// </summary>
        void TryReadCurrentDialogue()
        {
            if (allowExecNext == false) return;

            if (story.HasDialogueToFetch()) StartCoroutine(IEPrintDialogue());
            else OnStoryEnds();
        }

        /// <summary>
        /// Call this to exec next dialogue manually only.
        /// This call will interrupt dialogue reading if it still playing.
        /// </summary>
        public void NextDialogue()
        {
            // if (isAutoMode) return;

            if (isPlaying) interuptAuto = true;
            else if (allowExecNext) TryReadCurrentDialogue();
        }

        /// <summary>
        /// Print sentence with typing animation typing
        /// </summary>
        /// <returns></returns>
        IEnumerator IEPrintDialogue()
        {
            allowExecNext = false;

            currDialogue = story.GetCurrDialogue();

            // Assign Avatar to avatar pool and get the current speaking avatar based on current dialogue
            if (avatarPoolDict.ContainsKey(currDialogue.charAddressKey) == false)
            {
                AssignAvatarToPool(storyPackage.FindCharByAddressKey(currDialogue.charAddressKey));
            }
            CharacterAvatar currAvatar = avatarPoolDict[currDialogue.charAddressKey];

            // Display avatars
            DisplayCharacterImages(currDialogue);

            // Display dialogue
            speaker.text = currAvatar.GetCharName();
            
            string sentence = currDialogue.sentence;
            int sentenceLen = sentence.Length;
            message.text = "";
            isPlaying = true;
            for (int i = 0; i < sentenceLen; i++)
            {
                if (interuptAuto)
                {
                    message.text = sentence;
                    break;
                }
                message.text += sentence[i];
                yield return new WaitForSeconds(printingSpeed);
            }
            isPlaying = false;
            interuptAuto = false;

            // Display branch if any
            if (currDialogue.HasBranch())
            {
                yield return new WaitForSeconds(readBreakTime / 2f);
                DisplayBranchOptions();
            }
            else
            {
                story.ShiftToNextDialogue();
                ExecAutoMode();
            }
        }

        void OnStoryEnds()
        {
            StoryManager.Instance.SaveStoryProgress();
            StartCoroutine(IFadeOutAnim(null));
        }

        void DisplayBranchOptions()
        {
            canvasAC.Play("branch_in");

            int len = currDialogue.branches.Count;

            if (len > branchButtons.Length)
            {
                throw new System.InvalidOperationException("Branches out of bound. Can't display the interfaces");
            }

            for (int i = 0; i < len; i++)
            {
                branchButtons[i].gameObject.SetActive(true);
                branchButtons[i].Find("option_btn").Find("option_txt").GetComponent<TMP_Text>().text = currDialogue.branches[i].sentence;

                int index = i;
                branchButtons[i].Find("option_btn").GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ChooseBranch(index)); });
            }
        }

        void DisplayCharacterImages(Dialogue dialogue)
        {
            // Clear display container
            while(avatarDisplayCont.childCount > 0)
            {
                avatarDisplayCont.GetChild(0).SetParent(avatarPoolCont);
            }

            List<string> group = dialogue.GetSpeakerGroup();
            int len = group.Count;

            // Check if out of bound
            if (len > maximumDisplayedAvatar)
            {
                Debug.LogWarning("Warning: available container image is less than number of images to set");
                return;
            }

            // Set images
            CharacterAvatar avatar;
            for (int i = 0; i < len; i++)
            {
                avatar = avatarPoolDict[group[i]];

                if (avatar.GetAddressKey() == "me") continue;

                avatar.transform.SetParent(avatarDisplayCont);
                avatar.transform.localScale = Vector3.one;

                if (dialogue.charAddressKey == avatar.GetAddressKey())
                {
                    avatar.TintImage(Color.white);

                    // Play animation
                    avatar.PlayAnim(currDialogue.speakerAnim);
                }
                else avatar.TintImage(charDisabledColor);
            }
        }

        void ExecAutoMode()
        {
            if (isAutoMode == false)
            {
                allowExecNext = true;
                return;
            }
            StartCoroutine(IExecAutoMode());
        }

        IEnumerator IExecAutoMode()
        {
            yield return new WaitForSeconds(readBreakTime);
            allowExecNext = true;

            TryReadCurrentDialogue();
        }

        public void ToggleDoubleSpeed()
        {
            // Toggle Speed
            isDoubleSpeed = !isDoubleSpeed;
            PlayerPrefs.SetInt(DOUBLE_SPEED, isDoubleSpeed ? 1 : 0);

            UpdatePlayerPrefs();
            RepaintUI();
        }

        void UpdatePlayerPrefs()
        {
            isAutoMode = PlayerPrefs.GetInt(AUTO_MODE, 0) == 0 ? false : true;
            isDoubleSpeed = PlayerPrefs.GetInt(DOUBLE_SPEED, 0) == 0 ? false : true;

            Time.timeScale = isDoubleSpeed ? 2f : 1f;
        }

        void ResetPlayerPrefs()
        {
            Time.timeScale = 1f;
        }

        public void ToggleAutoMode()
        {
            // Toggle Auto
            isAutoMode = !isAutoMode;
            PlayerPrefs.SetInt(AUTO_MODE, isAutoMode ? 1 : 0);

            if (isAutoMode && allowExecNext) TryReadCurrentDialogue();

            RepaintUI();
        }

        void AssignAvatarToPool(CharacterAvatar avatar)
        {
            GameObject obj = Instantiate(avatar.gameObject, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(avatarPoolCont);

            avatarPoolDict.Add(avatar.GetAddressKey(), obj.GetComponent<CharacterAvatar>());
        }

        void ClearAvatarPool()
        {
            foreach(Transform obj in avatarDisplayCont) Destroy(obj.gameObject);

            foreach(Transform obj in avatarPoolCont) Destroy(obj.gameObject);

            avatarPoolDict.Clear();
        }

        IEnumerator ChooseBranch(int index)
        {
            foreach(Transform btn in branchButtons)
            {
                btn.GetComponent<Animator>().Play("branch_out");
            }

            yield return new WaitForSeconds(0.3f);
            ClearBranchOptions();

            int temp_chosen = story.ChooseBranch(index).chosenBranchIndex;

            // Set chosen branch index if there any branch chosen
            if (temp_chosen != -1) chosenBranchIndex = temp_chosen; 

            story.ShiftToNextDialogue();

            allowExecNext = true;

            TryReadCurrentDialogue();
        }

        void ClearBranchOptions()
        {
            foreach (Transform btn in branchButtons)
            {
                btn.gameObject.SetActive(false);
                btn.Find("option_btn").GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        IEnumerator IOpenMenu()
        {
            canvasAC.Play("fade_out", -1, 0f);
            yield return new WaitForSeconds(0.5f);
            StoryManager.Instance.OpenArchive();
            StopDriver();
        }

        public void StartDriver()
        {
            StoryManager.Instance.SaveStoryProgress();
            SetupDriver();
            StartCoroutine(IFadeInAnim(null));
        }

        public void StopDriver()
        {
            ResetPlayerPrefs();

            allowExecNext = false;
            StopAllCoroutines();
        }

        public void ResumeDriver()
        {
            UpdatePlayerPrefs();

            allowExecNext = true;

            ClearBranchOptions();
            TryReadCurrentDialogue();

            StartCoroutine(IFadeInAnim(null));
        }

        public IEnumerator IFadeInAnim(object param)
        {
            canvasAC.Play("fade_in", -1, 0f);
            yield return new WaitForSeconds(0.5f);
        }

        public IEnumerator IFadeOutAnim(object param)
        {
            canvasAC.Play("fade_out", -1, 0f);
            yield return new WaitForSeconds(0.5f);

            currDialogue = null;

            // Discover branches
            package.status = PackageStatus.COMPLETED;
            package.DiscoverBranches();

            // If the package has no branches, then the story ends
            if (package.branches.Count <= 0)
            {
                Debug.Log("The Story Stream ends!");
            }
            // Else load the next story package
            else
            {
                // Load next branch
                package = package.branches[chosenBranchIndex];
                StoryManager.Instance.LoadStory(package);
                if (archive.FindPackageByAddressKey(package.packAddressKey) is MemoryStory)
                {
                    package.status = PackageStatus.COMPLETED;
                    StopDriver();
                    StoryManager.Instance.OpenArchive();
                }
            }
        }
    }
}