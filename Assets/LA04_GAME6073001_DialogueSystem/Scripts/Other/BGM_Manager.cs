using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    public class BGM_Manager : MonoBehaviour
    {
        private static BGM_Manager instance;
        public static BGM_Manager Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<BGM_Manager>();
                return instance;
            }
        }

        public AudioSource audioSource;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}