using System.Collections.Generic;
using System.Linq;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class ReplicaMessageBox : MonoBehaviourExtBind
    {
        public Canvas hideShowCanvas;
        public Text messageText;
        public float waitTime = 2f;


        public class ReplicaItem
        {
            public string message;
            public float showTime;
        }

        [OnRefresh(0.2f)]
        public void OnRefreshBox()
        {
            if(Path.IsPlaying || PlayerReplicaQueue.Count == 0) return;

            var r = PlayerReplicaQueue.Peek();
            
            hideShowCanvas.gameObject.SetActive(true);
            messageText.text = r.message;

            Path = CPath.Create()
                .Wait(r.showTime)
                .Action(() => hideShowCanvas.gameObject.SetActive(false))
                .Wait(waitTime)
                .Action(() => PlayerReplicaQueue.Dequeue());
        }
        
        public Queue<ReplicaItem> PlayerReplicaQueue = new Queue<ReplicaItem>();

        [Bind]
        public void PlayerReplicaMessage(string message, float showTime)
        {
            if(PlayerReplicaQueue.Any(x => x.message == message)) return;
            
            PlayerReplicaQueue.Enqueue(new ReplicaItem()
            {
                message = message,
                showTime = showTime
            });
        } 
    }
}
