using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FrontEnd.Model.ViewModel;

namespace FrontEnd.UserControl.Popup
{
    /// <summary>
    /// Interaction logic for ReconnectToGame.xaml
    /// Popup windows to ask about reconnection to a disconnect game
    /// </summary>
    public partial class ReconnectToGame : Window
    {
        public ReconnectToGame(ReconnectViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            if (context.CloseAction == null)
                context.CloseAction = new Action(this.Close);
        }
    }
}
