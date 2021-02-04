using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class PlayerOilValue : MonoBehaviourExtBind
    {
        public Text value;
        public RectTransform parent;
        
        [OnEnable]
        public void onEnable()
        {
            OnOilValueChanged();
            OnHasOilChanged();
        }

        [Bind]
        public void OnOilValueChanged()
        {
            value.text = Model.GetInt("OilValue").ToString();
        }

        [Bind]
        public void OnHasOilChanged()
        {
            if(Model.GetBool("HasOil"))
                parent.gameObject.SetActive(true);
            else 
                parent.gameObject.SetActive(false);
        }
    }
}
