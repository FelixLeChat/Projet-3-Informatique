﻿using FrontEnd.Core.Event;
using FrontEnd.StateMachine.Core;
using FrontEnd.UserControl;

namespace FrontEnd.StateMachine.State
{
    public class OptionMenuState : IState
    {
        public void EnterState()
        {
            var window = new OptionMenu();
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
