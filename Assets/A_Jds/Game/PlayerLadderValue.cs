using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

namespace A_Jds.Game
{
    public class PlayerLadderValue : MonoBehaviourExtBind
    {
        public RectTransform parent;

        [OnEnable]
        public void onEnable()
        {
            OnHasLadderChanged();
        }
        
        [Bind]
        public void OnHasLadderChanged()
        {
            if(Model.GetBool("HasLadder"))
                parent.gameObject.SetActive(true);
            else 
                parent.gameObject.SetActive(false);
        }
    }
}
