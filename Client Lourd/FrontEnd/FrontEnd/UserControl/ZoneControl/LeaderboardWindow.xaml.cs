using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using FrontEndAccess.APIAccess;
using Models.Database;

namespace FrontEnd.UserControl.ZoneControl
{
    /// <summary>
    /// Interaction logic for LeaderboardWindow.xaml
    /// </summary>
    public partial class LeaderboardWindow
    {
        private ZoneListWindow _zoneList;
        public LeaderboardWindow(string mapHashId, ZoneListWindow zoneList)
        {
            InitializeComponent();
            _zoneList = zoneList;

            var initializeThread = new Thread(() =>
            {
                // set profile (will set stats and achievements)
                Initialize(mapHashId);
            });
            initializeThread.Start();

            // Change Title
            MainWindow.Instance.SwitchTitle("Tableau des Champions");
        }

        private static readonly ObservableCollection<LeaderModel> _leaderEntries = new ObservableCollection<LeaderModel>(); 
        public void Initialize(string mapHashId)
        {
            var leaderboard = LeaderboardAccess.Instance.GetLeaderboard(mapHashId);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                _leaderEntries.Clear();
                var entry = LeaderboardModelHelper.GetOrderedLeaders(leaderboard);
                foreach (var leaderModel in entry)
                {
                    _leaderEntries.Add(leaderModel);
                }
                ListLeaderboardEntry.ItemsSource = _leaderEntries;
                CollectionViewSource.GetDefaultView(_leaderEntries).Refresh();
            }));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Program.MainWindow.SwitchScreen(_zoneList);
        }
    }
}
