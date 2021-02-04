using AxGrid;
using AxGrid.Base;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class PlayerStateCounter : MonoBehaviourExtBind
    {
        public string counterName;
        public Image counter;
        public Text counterText;
        public float currentValue = 0;
        public float maxValue;

        
    }
}
