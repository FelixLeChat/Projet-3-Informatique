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
    /// Interaction logic for InviteFriendPopup.xaml
    /// </summary>
    public partial class InviteFriendPopup : Window
    {
        public InviteFriendPopup(InviteFriendsViewModel context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
