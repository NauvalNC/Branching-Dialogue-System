using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    public class StoryArchiveUnpacker
    {
        public ArchivePackage package = null;
        TextAsset jsonFile;

        public StoryArchiveUnpacker(TextAsset jsonFile)
        {
            this.jsonFile = jsonFile;
            Setup();
        }

        void Setup()
        {
            package = JsonUtility.FromJson<ArchivePackage>(jsonFile.text);
        }
    }
}