using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem_2301906331
{
    public class MemoryDriver : MonoBehaviour, IStoryDriver
    {
        [HideInInspector] public StoryArchivePackage archive;
        [HideInInspector] public ArchivePackage package;

        [Header("User Interfaces")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private TMP_Text memoryTitle;
        [SerializeField] private Image memoryImg;

        private Animator canvasAC;

        private void Start()
        {
            canvasAC = canvas.GetComponent<Animator>();
        }

        void DisplayMemory()
        {
            MemoryStory memory = (MemoryStory) archive.FindPackageByAddressKey(package.packAddressKey);
            memoryTitle.text = memory.GetStoryTitle();
            memoryImg.sprite = memory.GetMemory();
        }

        public void ResumeDriver()
        {
            canvas.SetActive(true);
        }

        public void StartDriver()
        {
            canvas.SetActive(true);
            DisplayMemory();
            StartCoroutine(IFadeInAnim(null));
        }

        public void StopDriver()
        {
            StartCoroutine(IFadeOutAnim(null));
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
            canvas.SetActive(false);
        }
    }
}