using System.Collections.Generic;
using AxGrid.Base;
using UnityEngine;

namespace A_Jds.Game
{
    public class CitizensReplicasQueue : MonoBehaviourExtBind
    {
        public Queue<CitizenReplicaBox> CitizenReplicaBoxes = new Queue<CitizenReplicaBox>();
        
        [OnRefresh(0.17f)]
        public void RefreshReplicas()
        {
            if(CitizenReplicaBoxes.Count == 0) return;
            CitizenReplicaBoxes.Dequeue().ShowReplica();
        }
    }
}
