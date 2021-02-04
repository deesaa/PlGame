using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine.UI;

namespace A_Jds.Game.States
{
    [State("FirstGameShow")]
    public class FirstGameShowState : FSMState
    {
        [Enter]
        public void OnEnter()
        {
            Parent.Change("FirstPlay");
        }
        
    }
}