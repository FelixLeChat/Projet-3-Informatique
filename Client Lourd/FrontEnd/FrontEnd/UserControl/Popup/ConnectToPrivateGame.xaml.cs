using System;
using System.Windows;
using System.Windows.Controls;
using FrontEnd.Model.ViewModel;

namespace FrontEnd.UserControl.Popup
{
    /// <summary>
    /// Interaction logic for ConnectToPrivateGame.xaml
    /// </summary>
    public partial class ConnectToPrivateGame : Window
    {
        private ConnectPrivateGameViewModel _context;

        public ConnectToPrivateGame(ConnectPrivateGameViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            if (context.CloseAction == null)
                context.CloseAction = new Action(this.Close);
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var context = DataContext as ConnectPrivateGameViewModel;

            if (context == null)
            {
                throw new Exception("The datacontext of this view should be ConnectPrivateGameViewModel");
            }
            var passwordBox = sender as PasswordBox;
            context.Password = passwordBox.Password;
        }
    }
}
