using A_Jds.Game.States;
using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using UnityEngine;

namespace A_Jds.Game
{
    public class GameMain : MonoBehaviourExt
    {
        [OnStart]
        public void Create()
        {
            Settings.Fsm = new FSM();
            Settings.Fsm.Add(new InitState());
            Settings.Fsm.Add(new FirstGameShowState());
            Settings.Fsm.Add(new FirstPlayState());
            Settings.Fsm.Add(new StartNewGameState());
            
            Settings.Fsm.Add(new GamePlayState());
            Settings.Fsm.Add(new GameOverState());
        }

        [OnStart]
        public void StartFSM()
        {
            Settings.Fsm.Start("Init");
            
        }

        [OnUpdate]
        public void UpdateFsm()
        {
            Settings.Fsm.Update(Time.deltaTime);
        }
    }
}
