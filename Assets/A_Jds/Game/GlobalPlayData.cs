using System.Collections.Generic;
using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class GlobalPlayData : MonoBehaviourExtBind
    {
        public float predawnTime;

        public int moneyPerLamp = 1;
        public int moneyPerFood = 6;
        public int oilPerLamp = 3;

       // public int maxCitizensAwake = 10;

        public int FullDayTime
        {
            get => Model.GetInt("FullDayTime");
            set => Model.Set("FullDayTime", value);
        }
        
        public int MaxCitizensAwake
        {
            get => Model.GetInt("MaxCitizensAwake");
            set => Model.Set("MaxCitizensAwake", value);
        }
        
        
        public int CitizensAwakeCount
        {
            get => Model.GetInt("CitizensAwakeCount");
            set => Model.Set("CitizensAwakeCount", value);
        }

        public int DayNumber
        {
            get => Model.GetInt("DayNumber");
            set => Model.Set("DayNumber", value);
        }
        
        public int CurrentTime
        {
            get => Model.GetInt("CurrentTime");
            set => Model.Set("CurrentTime", value);
        }
        
        public int SleepTime
        {
            get => Model.GetInt("SleepTime");
            set => Model.Set("SleepTime", value);
        }
        
        public int LampsChoke
        {
            get => Model.GetInt("LampsChoke");
            set => Model.Set("LampsChoke", value);
        }


        public Player player => Model.Get<Player>("Player");

        public List<Lamp> lamps = new List<Lamp>();
        public List<Citizen> citizens = new List<Citizen>();

        [OnRefresh(1f)]
        public void UpdateDaytime()
        {
            if(!Model.GetBool("IsGameStarted")) return;
            
            CurrentTime++;
            
            //Model.EventManager.Invoke("TimeClockUpdate", (float)CurrentTime/FullDayTime);
        }

        [Bind]
        public void OnCitizensAwakeCountChanged()
        {
            if(CitizensAwakeCount >= MaxCitizensAwake)
                Settings.Fsm.Invoke("CitizensGameOver");
        }

        [Bind]
        public void PlayerSleep()
        {
            Log.Debug("Global Player Sleep");

            DayNumber++;
            CurrentTime = 1;
            
            player.MoneyValue += LampsChoke * moneyPerLamp;

            LampsChoke = 0;

            Model.EventManager.Invoke("PlayerBecomeHuman");
            Model.EventManager.Invoke("StartNewDay");
            StartNewDay();
        }

        [Bind]
        public void StartNewDay()
        {
            CurrentTime = 1;
            CitizensAwakeCount = 0;
            LampsChoke = 0;
            FullDayTime = 100;
            MaxCitizensAwake = 25;
        }

        [Bind]
        public void RestartPlayData()
        {
            CurrentTime = 1;
            FullDayTime = 100;
            MaxCitizensAwake = 25;
            SleepTime = 40;
            DayNumber = 0;
            moneyPerFood = 1;
            moneyPerLamp = 1;
            oilPerLamp = 3;
            CitizensAwakeCount = 0;
            LampsChoke = 0;
        } 
    }
}
