using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;

namespace A_Jds.Game
{
    public class NewGameAnimation : MonoBehaviourExtBind
    {
        public float waitTime = 13.1f;
        
        [Bind]
        public void NewGameAnimationShow()
        {
            //Model.Set("IsGameStarted", true);
           // Model.Set("TimeLock", true);
            //TODO:
            //Settings.Fsm.Invoke("NewGameAnimationShown");
            Path = new CPath()
                .Action(() => Animator.SetBool("Show", true))
                .Wait(waitTime)
                .Action(() => Settings.Fsm.Invoke("NewGameAnimationShown"));
        }
    }
}
