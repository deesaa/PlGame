using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class PlayerHungerValue : MonoBehaviourExtBind
    {
        public Image counter;
        public float currentHungerValue = 0;
        private float maxHungerValue = 250;
        

        [OnRefresh(1f)]
        public void OnHungerRefresh()
        {
            currentHungerValue++;
            
            counter.fillAmount = ( currentHungerValue / maxHungerValue);

            if (currentHungerValue > maxHungerValue)
            {
                Settings.Fsm.Invoke("DeathStarvation");
            }
        }

        [Bind]
        public void RestartPlayData()
        {
            currentHungerValue = 0;
        }
    }
}
