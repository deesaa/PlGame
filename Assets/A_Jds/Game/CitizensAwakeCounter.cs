using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class CitizensAwakeCounter : MonoBehaviourExtBind
    {
        public Text text;

        public int CitizensAwakeCount
        {
            get => Model.GetInt("CitizensAwakeCount");
            set => Model.Set("CitizensAwakeCount", value);
        }
        
        public int MaxCitizensAwake
        {
            get => Model.GetInt("MaxCitizensAwake");
            set => Model.Set("MaxCitizensAwake", value);
        }

        [Bind]
        public void StartNewDay()
        {
            Log.Debug($"CitizensAwakeCounter {CitizensAwakeCount}");
            text.text = "";
        }

        [Bind]
        public void OnCitizensAwakeCountChanged()
        {
            Log.Debug($"CitizensAwakeCounter {CitizensAwakeCount}");
           
            int value = CitizensAwakeCount;
            int maxValue = MaxCitizensAwake;
            
            if(maxValue - value <= 0)
                text.text = "Ну все, пиздец";
            else
                text.text = $"Еще {maxValue - value} разбудить и мне точно конец";
            
        }
    }
}
