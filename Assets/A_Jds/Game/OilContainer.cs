using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace A_Jds.Game
{
    public class OilContainer : MonoBehaviourExtBind
    {

        public bool isDropped;
        
        [OnEnable]
        public void onEnable()
        {
            
        }
        
        [Bind]
        public void PlayerDropLadder()
        {
            Animator.SetBool("Drop", true); 
            isDropped = true;
        }
        
        [Bind]
        public void RestartPlayData()
        {
            Destroy(this.gameObject);
        }
    }
}
