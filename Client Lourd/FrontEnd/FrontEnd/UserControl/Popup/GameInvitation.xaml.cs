using System;
using System.Windows;
using FrontEnd.Model.ViewModel;

namespace FrontEnd.UserControl.Popup
{
    /// <summary>
    /// Interaction logic for GameInvitation.xaml
    /// </summary>
    public partial class GameInvitation : Window
    {
        public GameInvitation(GameInvitationViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            if (context.CloseAction == null)
                context.CloseAction = new Action(this.Close);
        }
    }
}
