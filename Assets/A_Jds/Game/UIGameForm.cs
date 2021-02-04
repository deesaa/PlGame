using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace A_Jds.Game
{
    public class UIGameForm : MonoBehaviourExtBind
    {
        public RectTransform showHideObject;
        
        [OnEnable]
        public void onEnable()
        {
            OnIsGameStartedChanged();
        }

        [Bind]
        public void OnIsGameStartedChanged()
        {
            if (Model.GetBool("IsGameStarted"))
            {
                showHideObject.gameObject.SetActive(true);
            }
            else
            {
                showHideObject.gameObject.SetActive(false);
            }
        }
    }
}
