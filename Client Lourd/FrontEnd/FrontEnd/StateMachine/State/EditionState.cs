using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Core.Helper;
using FrontEnd.StateMachine.Core;
using FrontEnd.UserControl;
using FrontEnd.UserControl.Winform;


namespace FrontEnd.StateMachine.State
{
    class EditionState : IState
    {
        private OpenGlPanel _windowOpengl;

        public void EnterState()
        {
            _windowOpengl = new OpenGlPanel();

            var editionZone = ConfigHelper.ConvertZoneName("zoneJeuDefaut.xml");
            NativeFunction.ouvrirPartieTest(editionZone, editionZone.Length);
            NativeFunction.dessinerOpenGL();


            _windowOpengl.SelectMode(IntegratedOpenGl.Mode.ModeEditeur);
            Program.MainWindow.SwitchScreen(_windowOpengl);
            Program.MainWindow.Hide();
        }

        public void ExitState()
        {
            _windowOpengl?.Dispose();

            Program.MainWindow.Show();
        }

        public void Run(double deltaTime)
        {
            _windowOpengl.Run(deltaTime);
        }

        public void Notice(IEvent triggeredEvent)
        {

        }

        public bool IsInTestMode()
        {
            return _windowOpengl.CurrentMode == IntegratedOpenGl.Mode.ModeTest;
        }
    }
}
