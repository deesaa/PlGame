using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace A_Jds.Game
{
    public class Clock : MonoBehaviourExtBind
    {
        public RectTransform arrow;

        public float gameStartArrowAngle;
        
        public float gameWarnArrowAngle;
        public float sunriseArrowAngle;
        public float canSleepArrowAngle;

        public float currentAngle;

        public bool sunriseWarnSend = false;
        public bool sunriseEventSend = false;


        [Bind]
        public void OnCurrentTimeChanged()
        {
            int currentTime = Model.GetInt("CurrentTime");
            int fullDayTime = Model.GetInt("FullDayTime");
            
           Log.Debug($"Clock {currentTime} {fullDayTime}"); 
            
            if(fullDayTime <= 0) return;

            currentAngle = gameStartArrowAngle + (-360 *  currentTime / fullDayTime);
            arrow.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);

            if (currentAngle <= canSleepArrowAngle)
                Model.Set("CanSleep", true);

            if (currentAngle <= gameWarnArrowAngle && !sunriseWarnSend)
            {
                Model.EventManager.Invoke("SunriseWarn");
                sunriseWarnSend = true;
            }

            if (currentAngle <= sunriseArrowAngle && !sunriseEventSend)
            {
                Model.EventManager.Invoke("OnSunrise");
                sunriseEventSend = true;
            }
        }

        [Bind]
        public void StartNewDay()
        {
            arrow.rotation = Quaternion.AngleAxis(gameStartArrowAngle, Vector3.forward);
            currentAngle = gameStartArrowAngle;
            sunriseWarnSend = false;
            sunriseEventSend = false;
        }

        [Bind]
        public void RestartPlayData()
        {
            arrow.rotation = Quaternion.AngleAxis(gameStartArrowAngle, Vector3.forward);
            currentAngle = gameStartArrowAngle;
            sunriseWarnSend = false;
            sunriseEventSend = false;
        }
    }
}
