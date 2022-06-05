using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    [System.Serializable]
    public class ArchivePackage
    {
        public string packAddressKey;
        [SerializeField] private int ID = -1;
        public int mergePointID = -1;
        public PackageStatus status = PackageStatus.NOT_OBTAINED;
        /// <summary>
        /// Is current package on story archive?
        /// </summary>
        public bool isCurrentPackage = false;
        public List<ArchivePackage> branches = new List<ArchivePackage>();


        public ArchivePackage(int ID)
        {
            this.ID = ID;
        }

        public ArchivePackage(int ID, string packAddressKey)
        {
            this.ID = ID;
            this.packAddressKey = packAddressKey;
        }

        public int GetID() { return this.ID; }

        public void DiscoverBranches()
        {
            foreach(ArchivePackage pkg in branches)
            {
                if (pkg.status == PackageStatus.NOT_OBTAINED)
                    pkg.status = PackageStatus.DISCOVERED;
            }
        }
    }

    [System.Serializable]
    public enum PackageStatus
    {
        NOT_OBTAINED,
        DISCOVERED,
        VISITED,
        COMPLETED
    }
}
