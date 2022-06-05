using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    [CreateAssetMenu(fileName = "New Memory Story", menuName = "Dialogue System/New Memory Story", order = 2)]
    public class MemoryStory : StoryPackage
    {
        [SerializeField] private Sprite memoryImg;

        public Sprite GetMemory() { return memoryImg; }
    }
}