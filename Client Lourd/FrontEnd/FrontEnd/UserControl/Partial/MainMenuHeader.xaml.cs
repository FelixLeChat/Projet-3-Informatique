using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;

namespace FrontEnd.UserControl.Partial
{
    /// <summary>
    /// Interaction logic for MainMenuHeader.xaml
    /// </summary>
    public partial class MainMenuHeader
    {
        private readonly EventManager _eventManager;
        public MainMenuHeader()
        {
            InitializeComponent();
            _eventManager = EventManager.Instance;
        }

        private void MainMenuButton_Click(object sender, MouseButtonEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }
    }
}
