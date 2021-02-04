using System;
using System.Collections.Generic;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace A_Jds.Game
{
    public class Home : MonoBehaviourExtBind
    {
        public Citizen citizenPrototype;

        public int citizensMaxCount;
        public int citizensAwake;

        public Transform spawnPoint;
        
        [OnAwake]
        public void OnAwake()
        {
            
        }

        [Bind]
        public void OnNoize(float value)
        {
            int citizensForAwake = (int) (citizensMaxCount * value);

            citizensForAwake = Mathf.Clamp(citizensForAwake, 0, citizensMaxCount-citizensAwake);

            for (int i = 0; i < citizensForAwake; i++)
            {
                var newCitizen = Instantiate(citizenPrototype);
                newCitizen.citizenHome = this;
                newCitizen.transform.position = spawnPoint.position;
            }
        }
        
        [Bind]
        public void RestartPlayData()
        {
            citizensAwake = 0;
        } 
    }
}
