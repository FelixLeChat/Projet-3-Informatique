using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using FrontEnd.AsyncLoading;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEndAccess.APIAccess;
using Models.Database;
using Models.Frontend;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl.ZoneControl
{
    /// <summary>
    /// Interaction logic for ZoneListWindow.xaml
    /// </summary>
    public partial class ZoneListWindow
    {
        private const string DefaultImagePath = "/FrontEnd;component/Ressources/UI_Images/banner.png";
        private readonly EventManager _eventManager;
        public enum Type
        {
            User,
            Public,
            Friend
        };

        private readonly ObservableCollection<ZoneModelListItem> _myZones = new ObservableCollection<ZoneModelListItem>();
        private readonly ObservableCollection<ZoneModelListItem> _myFriendsZones = new ObservableCollection<ZoneModelListItem>();
        private readonly ObservableCollection<ZoneModelListItem> _publicZones = new ObservableCollection<ZoneModelListItem>();
        private Type _currentType;
        public ZoneListWindow(Type type = Type.User)
        {
            InitializeComponent();
            _eventManager = EventManager.Instance;

            // Get list of map
            _currentType = type;
            Initialize(_currentType);

            // Change Title
            MainWindow.Instance.SwitchTitle("Liste des Zones");
        }

        private void Initialize(Type type = Type.User)
        {
            MainWindow.Instance.PanelLoading = true;
            switch (type)
            {
                case Type.User:
                    ShowMyZones();
                    break;
                case Type.Public:
                    ShowMyFriendsZones();
                    break;
                case Type.Friend:
                    ShowAllPublicZones();
                    break;
            }
        }

        private void ShowMyZones()
        {
            if (_myZones.Count == 0)
                UpdateZones(ZoneAccess.Instance.GetMyZones(), _myZones);
            else
                ListUserMap.ItemsSource = _myZones;
            

            MainWindow.Instance.PanelLoading = false;
            _currentType = Type.User;
        }

        private void ShowMyFriendsZones()
        {
            if (_myFriendsZones.Count == 0)
                UpdateZones(ZoneAccess.Instance.GetMyFriendsZones(), _myFriendsZones);
            else
                ListUserMap.ItemsSource = _myFriendsZones;

            MainWindow.Instance.PanelLoading = false;
            _currentType = Type.Friend;
        }

        private void ShowAllPublicZones()
        {
            if (_publicZones.Count == 0)
                UpdateZones(ZoneAccess.Instance.GetPublicZones(), _publicZones);
            else
            ListUserMap.ItemsSource = _publicZones;

            MainWindow.Instance.PanelLoading = false;
            _currentType = Type.Public;
        }

        private void UpdateZones(IEnumerable<MapModel> maps, ObservableCollection<ZoneModelListItem> currentMaps)
        {
            if (currentMaps.Count == 0)
            {
                foreach (var map in maps)
                {
                    currentMaps.Add(new ZoneModelListItem()
                    {
                        Name = map.Name,
                        Level = map.Level,
                        MapImage = DefaultImagePath,
                        HashId = map.HashId
                    });

                    var imageThread = new Thread(() =>
                    {
                        // set profile (will set stats and achievements)
                        GetImages(currentMaps);
                    });
                    imageThread.Start();
                }
            }
                ListUserMap.ItemsSource = currentMaps;
        }

        private void GetImages(ObservableCollection<ZoneModelListItem> zones)
        {
            foreach (var zone in zones.ToList())
            {
                var path = Path.GetTempPath() + zone.HashId + ".png";
                if (File.Exists(path))
                {
                    zone.MapImage = path;
                }
                else
                {
                    try
                    {
                        zone.MapImage = ZoneAccess.Instance.GetMapImage(zone.HashId);
                    }
                    catch (Exception)
                    {
                        zone.MapImage = DefaultImagePath;
                    }
                }
            }
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                ListUserMap.ItemsSource = zones;
                CollectionViewSource.GetDefaultView(zones).Refresh();
                MainWindow.Instance.PanelLoading = false;
            }));
        }

        private void MainMenuButton_Click(object sender, MouseButtonEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }

        private void ProfileButton_Click(object sender, MouseButtonEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.Profile });
        }

        private void FriendButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMyFriendsZones();
            DeleteMap.Visibility = Visibility.Collapsed;
        }

        private void PublicButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAllPublicZones();
            DeleteMap.Visibility = Visibility.Collapsed;
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMyZones();
            DeleteMap.Visibility = Visibility.Visible;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _myZones.Clear();
            _myFriendsZones.Clear();
            _publicZones.Clear();
            Initialize(_currentType);
        }

        private void LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ListUserMap.SelectedItems;
            if (selectedItem != null && selectedItem.Count > 0)
            {
                Program.MainWindow.SwitchScreen(new LeaderboardWindow(((ZoneModelListItem)selectedItem[0]).HashId, this));
            }
        }

        // Delete Maps
        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = ListUserMap.SelectedItems;
            if (selectedItem != null && selectedItem.Count > 0)
            {
                var hash = ((ZoneModelListItem) selectedItem[0]).HashId;
                ZoneAccess.Instance.DeleteMap(hash);
                
                // Synchronize the zones
                ZoneSynchronizer.SynchronizeZone();

                // Refresh visual
                RefreshButton_Click(null, null);
            }
        }
    }
}
