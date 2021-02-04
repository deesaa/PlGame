using AxGrid.Base;
using AxGrid.Model;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace A_Jds.Game
{
    public class FirstPlayReplicaTrigger : MonoBehaviourExtBind
    {
        public string text;
        public Collider2D area;
        public bool isShown = false;
        
        public void InvokeReplica()
        {
            if(isShown || !Model.GetBool("IsFirstPlay") || !Model.GetBool("IsGameStarted")) return;
            
            Model.EventManager.Invoke("PlayerReplicaMessage", text, 5f);
            isShown = true;
        }

        [Bind]
        public void RestartPlayData()
        {
            isShown = false;
        }
    }
}
