using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace A_Jds.Game.States
{
    [State("GameOver")]
    public class GameOverState : FSMState
    {
        [Enter]
        public void OnEnter()
        {
            Model.Set("IsFirstPlay", false);
            Model.Set("IsGameOver", true);
        }

        [Bind]
        public void OnOkayButton()
        {
            Parent.Change("StartNewGame");
        }
     }
}