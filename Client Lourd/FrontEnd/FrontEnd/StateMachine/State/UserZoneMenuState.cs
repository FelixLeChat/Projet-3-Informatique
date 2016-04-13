using FrontEnd.Core.Event;
using FrontEnd.StateMachine.Core;
using FrontEnd.UserControl.ZoneControl;

namespace FrontEnd.StateMachine.State
{
    class UserZoneMenuState : IState
    {
        public void EnterState()
        {
            var window = new ZoneListWindow();
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
