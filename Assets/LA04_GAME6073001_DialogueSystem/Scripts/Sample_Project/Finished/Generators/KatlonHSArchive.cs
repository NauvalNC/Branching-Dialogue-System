#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331 
{
    public class KatlonHSArchive : StoryArchiveBuilder
    {
        public override void BuildStoryArchive()
        {
            MAIN_PACK.packAddressKey = "the_encounter";

            ArchivePackage convenienceStore = new ArchivePackage(GetNextID(), "convenience_store");
            ArchivePackage newSchoolMemory = new ArchivePackage(GetNextID(), "new_school_memory");
            ArchivePackage thePresident = new ArchivePackage(GetNextID(), "the_president");

            ArchivePackage theNeighborhood = new ArchivePackage(GetNextID(), "the_neighborhood");
            theNeighborhood.branches.Add(convenienceStore);

            ArchivePackage myNewSchool = new ArchivePackage(GetNextID(), "my_new_school");
            ArchivePackage schoolstroll = new ArchivePackage(GetNextID(), "school_stroll");
            schoolstroll.branches.Add(thePresident);
            
            myNewSchool.branches.Add(newSchoolMemory);
            myNewSchool.branches.Add(schoolstroll);

            MAIN_PACK.branches.Add(theNeighborhood);
            MAIN_PACK.branches.Add(myNewSchool);
        }
    }
}

#endif