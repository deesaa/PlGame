using System.Security.Cryptography;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace A_Jds.Game
{
    public class Citizen : MonoBehaviourExtBind
    {
        public Player player => Model.Get<Player>("Player");

        public Home citizenHome; 

        public int CitizensAwakeCount
        {
            get => Model.GetInt("CitizensAwakeCount");
            set => Model.Set("CitizensAwakeCount", value);
        }

        [OnEnable]
        public void onEnable()
        {
            
        }

        [Bind]
        public void StartNewDay()
        {
            Destroy(this.gameObject);
            CitizensAwakeCount--;
        }
        
        public void CitizenAwake()
        {
            CitizensAwakeCount++;
        }
        
        [Bind]
        public void RestartPlayData()
        {
            Destroy(this.gameObject);
            CitizensAwakeCount = 0;
        } 
    }
}
