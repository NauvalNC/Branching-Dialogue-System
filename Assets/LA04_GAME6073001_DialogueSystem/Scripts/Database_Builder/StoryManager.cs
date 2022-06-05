using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DialogueSystem_2301906331
{
    public class StoryManager : MonoBehaviour
    {
        private static StoryManager instance;
        public static StoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<StoryManager>();
                }
                return instance;
            }
        }

        [Header("Setup")]
        [SerializeField] private StoryArchivePackage archive;
        private ArchivePackage rootPackage;
        [SerializeField] private string defaultStorySaveFilename = "katlon_hs";
        [SerializeField] private bool resetStoryProgress = false;

        [Header("Driver")]
        [SerializeField] private StoryArchiveDriver archiveDriver;
        [SerializeField] private DialogueDriver dialogueDriver;
        [SerializeField] private MemoryDriver memoryDriver;

        private bool isStoryActive = false;

        public void SetStoryArchive(StoryArchivePackage archive)
        {
            this.archive = archive;
        }

        private void Start()
        {
            archive.Refresh();

            // Load Package
            ArchivePackage temp = archiveDriver.LoadStoryProgress(defaultStorySaveFilename);
            if (temp != null && resetStoryProgress == false)
            {
                rootPackage = temp;
            } else
            {
                rootPackage = new StoryArchiveUnpacker(archive.GetArchiveJSONFile()).package;
            }

            archiveDriver.archive = archive;
            archiveDriver.rootPackage = rootPackage;

            archiveDriver.StartDriver();
        }

        public bool IsStoryActive() { return isStoryActive; }

        public void AbandonStory()
        {
            SetAllPackageInactive(rootPackage);
            isStoryActive = false;
        }

        public void LoadStory(ArchivePackage package)
        {
            SetAllPackageInactive(rootPackage);

            if (archive.FindPackageByAddressKey(package.packAddressKey) is MemoryStory)
            {
                AbandonStory();
                LoadMemory(package);
                return;
            }

            package.isCurrentPackage = true;
            isStoryActive = true;

            dialogueDriver.archive = archive;
            dialogueDriver.package = package;

            dialogueDriver.StartDriver();
        }

        public void LoadMemory(ArchivePackage package)
        {
            memoryDriver.archive = archive;
            memoryDriver.package = package;
            memoryDriver.StartDriver();
        }

        void SetAllPackageInactive(ArchivePackage root)
        {
            root.isCurrentPackage = false;
            foreach (ArchivePackage pk in root.branches) SetAllPackageInactive(pk);
        }

        public void OpenArchive()
        {
            archiveDriver.StartDriver();
        }

        public void ReturnToStoryDialogue()
        {
            dialogueDriver.ResumeDriver();
        }

        public void SaveStoryProgress()
        {
            archiveDriver.SaveStoryProgress(defaultStorySaveFilename);
        }
    }

    public interface IStoryDriver
    {
        public void StartDriver();
        public void StopDriver();
        public void ResumeDriver();

        public IEnumerator IFadeInAnim(object param);

        public IEnumerator IFadeOutAnim(object param);
    }
}