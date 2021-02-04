using AxGrid.FSM;

namespace A_Jds.Game.States
{
    [State("Init")]
    public class InitState : FSMState
    {
        [Enter]
        public void Enter()
        {
#if UNITY_EDITOR
            FSM.ShowFsmEnterState = true;
#endif
        }

        [One(0.1f)]
        public void GoToReady()
        {
            Model.Set("IsFirstPlay", true);
            Model.EventManager.Invoke("RestartPlayData");
            Parent.Change("StartNewGame");
        }
    }
}