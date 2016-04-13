using FrontEnd.Core.Event;
using FrontEnd.StateMachine.Core;
using FrontEnd.UserControl;

namespace FrontEnd.StateMachine.State
{
    class ProfileMenuState : IState
    {
        public void EnterState()
        {
            var window = new ProfileMenu();
            Program.MainWindow.SwitchScreen(window);
        }

        public void ExitState()
        {
        }

        public void Run(double deltaTime)
        {
        }

        public void Notice(IEvent triggeredEvent)
        {
        }
    }
}
