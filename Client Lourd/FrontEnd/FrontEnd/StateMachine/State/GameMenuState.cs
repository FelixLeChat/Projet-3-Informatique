using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game.Config;
using FrontEnd.Game.Wrap;
using FrontEnd.StateMachine.Core;
using FrontEnd.UserControl;

namespace FrontEnd.StateMachine.State
{
    public class GameMenuState : IState
    {
        public void EnterState()
        {
            var gameMenu = new GameMenu();
            Program.MainWindow.SwitchScreen(gameMenu);
        }

        public void ExitState()
        {
        }

        public void Run(double deltaTime)
        {
        }

        public void Notice(IEvent triggeredEvent)
        {
            var uiEvent = triggeredEvent as UiEvent;
            if (uiEvent != null)
            {
                HandleUiEvent(uiEvent);
            }
        }

        private void HandleUiEvent(UiEvent uiEvent)
        {
            switch (uiEvent.Info)
            {
                case UiEvent.EventInfo.OfflineGame:
                    EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.OfflineGame});
                    break;
                case UiEvent.EventInfo.OnlineGame:
                    EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.OnlineGame });
                    break;
                case UiEvent.EventInfo.Back:
                    var playMenu = new GameMenu();
                    Program.MainWindow.SwitchScreen(playMenu);
                    break;
            }
        }

    }
}
