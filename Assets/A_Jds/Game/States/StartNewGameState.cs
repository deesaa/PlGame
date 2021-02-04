using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine.UI;

namespace A_Jds.Game.States
{
    [State("StartNewGame")]
    public class StartNewGameState : FSMState
    {
        [Enter]
        public void OnEnter()
        {
            Model.EventManager.Invoke("RestartPlayData");
            
            if(Model.GetBool("IsFirstPlay"))
                Model.EventManager.Invoke("NewGameAnimationShow");
            else 
                Parent.Change("GamePlay");
        }

        [Bind]
        public void NewGameAnimationShown()
        {
            if(Model.GetBool("IsFirstPlay"))
                Parent.Change("FirstGameShow");
            else
                Parent.Change("GamePlay");
        }
    }
}