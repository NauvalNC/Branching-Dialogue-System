using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    [CreateAssetMenu(fileName = "New Story Archive", menuName = "Dialogue System/New Story Archive", order = 0)]
    public class StoryArchivePackage : ScriptableObject
    {
        [SerializeField] private string archiveTitle;
        [SerializeField] private TextAsset jsonFile;
        [SerializeField] private List<StoryPackage> packages;
        private Dictionary<string, StoryPackage> packDict = null;

        public string GetArchiveTitle()
        {
            return archiveTitle;
        }

        public TextAsset GetArchiveJSONFile()
        {
            return jsonFile;
        }

        /// <summary>
        /// Refresh data contained in the archive
        /// </summary>
        public void Refresh()
        {
            CreatePackageDictionary();
        }

        void CreatePackageDictionary()
        {
            packDict = null;
            packDict = new Dictionary<string, StoryPackage>();
            foreach (StoryPackage pk in packages)
            {
                if (packDict.ContainsKey(pk.GetAddressKey()) == false)
                    packDict.Add(pk.GetAddressKey(), pk);
            }
        }

        /// <summary>
        /// Return story package filtered by address key.
        /// </summary>
        /// <param name="addressKey"></param>
        /// <returns></returns>
        public StoryPackage FindPackageByAddressKey(string addressKey)
        {
            return packDict[addressKey];
        }
    }
}