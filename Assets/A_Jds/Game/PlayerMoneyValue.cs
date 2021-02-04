using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class PlayerMoneyValue : MonoBehaviourExtBind
    {
        public Text value;

        [OnEnable]
        public void onEnable()
        {
            OnMoneyValueChanged();
        }
        
        [Bind]
        public void OnMoneyValueChanged()
        {
            value.text = Model.GetInt("MoneyValue").ToString();
        } 
    }
}
