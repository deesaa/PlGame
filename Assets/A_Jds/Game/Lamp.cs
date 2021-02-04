using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace A_Jds.Game
{
    public class Lamp : MonoBehaviourExtBind
    {
        public bool isLit;

        public Light pointMain;
        public Light pointSub;

        public Transform ladderSprite;

        public Transform ladderStart;
        public Transform ladderEnd;

        public AudioSource audioSource;
        public AudioClip lampLit;
        public AudioClip chokeLamp;

        [OnEnable]
        public void onEnable()
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        public int LampsChoke
        {
            get => Model.GetInt("LampsChoke");
            set => Model.Set("LampsChoke", value);
        }


        public void ChokeLamp()
        {
            isLit = false;
            audioSource.clip = chokeLamp;
            audioSource.loop = false;
            audioSource.Play();
            LampsChoke++;
            Animator.SetBool("IsLit", false);
        }

        [Bind]
        public void RestartPlayData()
        {
            isLit = true;
            audioSource.clip = lampLit;
            audioSource.loop = true;
            audioSource.Play();
            Animator.SetBool("IsLit", true);
        }

        [Bind]
        public void StartNewDay()
        {
            isLit = true;
            audioSource.clip = lampLit;
            audioSource.loop = true;
            audioSource.Play();
            Animator.SetBool("IsLit", true);
        }
    }
}
