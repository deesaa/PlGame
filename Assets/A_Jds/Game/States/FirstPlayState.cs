using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine.UI;

namespace A_Jds.Game.States
{
    [State("FirstPlay")]
    public class FirstPlayState : FSMState
    {
        [Enter]
        public void OnEnter()
        {
            Parent.Change("GamePlay");
        }

        [Bind]
        public void AllTutorialStepsDone()
        {
        }
    }
}