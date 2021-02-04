using AxGrid.FSM;
using AxGrid.Model;

namespace A_Jds.Game.States
{
    [State("GamePlay")]
    public class GamePlayState : FSMState
    {
        [Enter]
        public void OnEnter()
        {
            Model.Set("IsGameStarted", true);
            Model.Set("IsGameOver", false);
            Model.EventManager.Invoke("StartNewDay"); 
        }

        [Bind]
        public void CitizensGameOver()
        {
            Model.Set("GameOverText", "Вас запиздили ногами");
            Parent.Change("GameOver");
        }
        
        [Bind]
        public void DeathStarvation()
        {
            Model.Set("GameOverText", "Вы здохли от голода");
            Parent.Change("GameOver");
        }
    }
}