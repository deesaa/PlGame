using System;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using AxGrid.Utils;
using UnityEngine;

namespace A_Jds.Game
{
    public class SunLightController : MonoBehaviourExtBind
    {
        public Light sunLight;

        public Color color1;
        public Color color2;
        public Color color3;
        public Color color4;
        public Color color5;

        public float intensityMin = 0.2f;
        public float intensityMax = 1.3f;

        public bool isDay = false;

        public Color targetColor;

        [Bind]
        public void OnCurrentTimeChanged()
        {
            if(isDay) return;
            
            float currentTime = Model.GetInt("CurrentTime");
            float fullDayTime = Model.GetInt("FullDayTime");

            float fullNightTime = fullDayTime * 0.5f;

            float nightValue = currentTime / fullNightTime;

            sunLight.intensity = Mathf.Lerp(intensityMin, intensityMax, nightValue);

            if (nightValue >= 0.7)
                ChangeLight(color3, color4);
            else if (nightValue >= 0.5)
                ChangeLight(color2, color3);
            else if (nightValue >= 0.3)
                ChangeLight(color1, color2);
        }

        [Bind]
        public void OnSunrise()
        {
            ChangeLight(sunLight.color, color5);
            sunLight.intensity = intensityMax;
            isDay = true;
        }

        private void ChangeLight(Color colorStart, Color colorEnd)
        {
            if(targetColor == colorEnd) return;
            targetColor = colorEnd;
            
            Vector3 vColorStart = new Vector3(colorStart.r, colorStart.g, colorStart.b);
            Vector3 vColorEnd = new Vector3(colorEnd.r, colorEnd.g, colorEnd.b);
            
            Path = new CPath().EasingLinear(1f, 0, 1, x =>
            {
                var nv = Vector3.Lerp(vColorStart, vColorEnd, x);
                sunLight.color = new Color(nv.x, nv.y, nv.z);
            });
        }

        [Bind]
        public void StartNewDay()
        {
            isDay = false;
            sunLight.intensity = intensityMin;
            sunLight.color = color1;
        }

        [Bind]
        public void RestartPlayData()
        {
            isDay = false;
            sunLight.intensity = intensityMin;
            sunLight.color = color1;
        }
    }
}
