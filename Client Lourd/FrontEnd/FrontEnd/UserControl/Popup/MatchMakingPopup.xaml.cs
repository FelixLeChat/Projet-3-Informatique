using System;
using System.Collections.Generic;
using System.Windows;
using FrontEnd.Model.ViewModel;

namespace FrontEnd.UserControl.Popup
{
    /// <summary>
    /// Interaction logic for MatchMakingPopup.xaml
    /// </summary>
    public partial class MatchMakingPopup : Window
    {
        public MatchMakingPopup(MatchMakingViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            if (context.CloseAction == null)
                context.CloseAction = new Action(this.Close);
        }
    }
}
