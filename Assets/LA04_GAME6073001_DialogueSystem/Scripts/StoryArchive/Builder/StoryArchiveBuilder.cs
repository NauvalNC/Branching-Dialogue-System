#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace DialogueSystem_2301906331
{
    public abstract class StoryArchiveBuilder : Builder
    {
        public ArchivePackage MAIN_PACK;

        public StoryArchiveBuilder()
        {
            MAIN_PACK = new ArchivePackage(GetNextID());
            MAIN_PACK.status = PackageStatus.VISITED;
        }

        /// <summary>
        /// Build your story archive here.
        /// </summary>
        public abstract void BuildStoryArchive();

        /// <summary>
        /// Generate JSON file of the story archive
        /// </summary>
        /// <param name="filePath"></param>
        public override void GenerateJSON(string filePath)
        {
            BuildStoryArchive();
            string jsonString = JsonUtility.ToJson(MAIN_PACK, true);
            System.IO.File.WriteAllText(filePath, jsonString);
        }
    }
}

#endif