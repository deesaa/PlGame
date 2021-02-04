using System.Collections.Generic;
using AxGrid.Base;
using UnityEngine;

namespace A_Jds.Game
{
    public class PlayerReplicasQueue : MonoBehaviourExtBind
    {
        public Queue<CitizenReplicaBox> PlayerReplicaBoxes = new Queue<CitizenReplicaBox>();
        
        [OnRefresh(0.2f)]
        public void RefreshReplicas()
        {
            if(PlayerReplicaBoxes.Count == 0) return;
            PlayerReplicaBoxes.Dequeue().ShowReplica();
        }
    }
}
