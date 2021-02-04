using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

namespace A_Jds.Game
{
    public class GameOverForm : MonoBehaviourExtBind
    {
        public Button okayButton;
        public Text text;
        public RectTransform showHideObject;
        
        [OnAwake]
        public void onAwake()
        {
            okayButton.onClick.AddListener(OnButtonOkay);
        }

        [OnEnable]
        public void onEnable()
        {
           // OnIsGameOverChanged();
        }

        [Bind]
        public void OnIsGameOverChanged()
        {
            if (Model.GetBool("IsGameOver"))
            {
                showHideObject.gameObject.SetActive(true);
                text.text = Model.GetString("GameOverText");
            }
            else
            {
                showHideObject.gameObject.SetActive(false);
            }
        }

        public void OnButtonOkay()
        {
            Settings.Fsm.Invoke("OnOkayButton");
        }

        [OnDestroy]
        public void onDisable()
        {
            okayButton.onClick.RemoveListener(OnButtonOkay);
        }
    }
}
