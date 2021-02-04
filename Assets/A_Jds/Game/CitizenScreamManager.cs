using System.Collections.Generic;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace A_Jds.Game
{
    public class CitizenScreamManager : MonoBehaviourExtBind
    {
        public List<AudioClip> citizensScreams;
        private AudioSource audioSource;

        [OnAwake]
        public void onAwake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        [Bind]
        public void CockMakeNoise()
        {
            if (Random.value >= 0.5 && !audioSource.isPlaying) 
            {
                audioSource.clip = citizensScreams[Random.Range(0, citizensScreams.Count)];
                audioSource.loop = false;
                audioSource.Play();
            }
        }

        [Bind]
        public void StartNewDay()
        {
            audioSource.Stop();
        }
    }
}
