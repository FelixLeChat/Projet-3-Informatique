using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEnd.Stats;
using FrontEndAccess.APIAccess;
using Models;
using Models.Database;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for UserFriendsMenu.xaml
    /// </summary>
    public partial class UserFriendsMenu
    {
        private static EventManager _eventManager;
        private List<BasicUserInfo> _users = new List<BasicUserInfo>();

        public UserFriendsMenu()
        {
            InitializeComponent();
            _eventManager = EventManager.Instance;

            // load all users
            var usersThread = new Thread(() =>
            {
                _users = ProfileAccess.Instance.GetAllPublicUserInfos();

                // remove current user
                _users = _users.Where(x => x.Username != User.Instance.Name).ToList();
                InitializeUsersList();
            });
            usersThread.Start();

            StartChatButtonGray.Visibility = Visibility.Visible;
            FriendProfileButtonGray.Visibility = Visibility.Visible;

            // Change Title
            MainWindow.Instance.SwitchTitle("Liste d'amis");
        }

        public void InitializeUsersList()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                AllUsers.Items.Clear();
                FriendList.Items.Clear();

                foreach (var user in _users)
                {
                    if (user.AreFriend)
                    {
                        // Add to friend list
                        FriendList.Items.Add(user.Username);
                    }
                    else
                    {
                        AllUsers.Items.Add(user.Username);
                    }
                }
            }));
        }

        private void SetFriendship(IEnumerable selectedItems, bool friend)
        {
            foreach (var index in from string selectedItem in selectedItems where selectedItem != null select _users.FirstOrDefault(x => x.Username == selectedItem) into user where user != null select _users.IndexOf(user))
            {
                _users[index].AreFriend = friend;
            }
            UpdateFriends();
            InitializeUsersList();

            // Daily to add friend
            if(friend)
                DailyManager.AchieveDaily(EnumsModel.DailyType.AddFriend);
        }

        private void UpdateFriends()
        {
            var updateUserThread = new Thread(() =>
            {
                var friends = _users.Where(user => user.AreFriend).ToList();
                FriendAccess.Instance.UpdateFriends(friends);
            });
            updateUserThread.Start();
        }

        #region State Transition
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.Profile });
        }
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }
        #endregion

        #region Friend Region
        private BasicUserInfo _selectedFriend;
        private void RemoveFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = FriendList.SelectedItems;
            if (selectedItems.Count > 0)
                SetFriendship(selectedItems, false);

            RemoveFriendButtonGray.Visibility = Visibility.Visible;
        }

        private void StartChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFriend != null)
            {
                ChatMenu.StartChatWithFriend(_selectedFriend.HashId, _selectedFriend.Username);
            }
        }

        private void ShowFriendProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFriend != null)
            {
                var profile = ProfileAccess.Instance.GetUserInfoPlease(_selectedFriend.HashId);
                if (profile != null && _selectedFriend.HashId != Profile.Instance.CurrentProfile.UserHashId)
                {
                    Program.MainWindow.SwitchScreen(new ProfileMenu(profile));
                }
            }
        }

        private void FriendList_OnLostFocus(object sender, RoutedEventArgs e)
        {
            DisableAllFriendButtons();
            RemoveFriendButtonGray.Visibility = Visibility.Visible;
        }

        private void DisableAllFriendButtons()
        {
            StartChatButtonGray.Visibility = Visibility.Visible;
            FriendProfileButtonGray.Visibility = Visibility.Visible;
        }

        private void FriendList_OnGotFocus(object sender, RoutedEventArgs e)
        {
            FriendList_SelectionChanged(sender, null);
        }
        private void FriendList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                DisableAllFriendButtons();
                return;
            }

            var name = (sender as ListBox)?.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(name))
            {
                DisableAllFriendButtons();
            }
            else
            {
                StartChatButtonGray.Visibility = Visibility.Collapsed;
                FriendProfileButtonGray.Visibility = Visibility.Collapsed;
                RemoveFriendButtonGray.Visibility = Visibility.Collapsed;
                _selectedFriend = _users.FirstOrDefault(x => x.Username == name);
            }

        }
        #endregion

        #region All Users Region
        private void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = AllUsers.SelectedItems;
            if (selectedItems.Count > 0)
                SetFriendship(selectedItems, true);

            AddFriendButtonGray.Visibility = Visibility.Visible;
        }

        private void AllUsers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                AddFriendButtonGray.Visibility = Visibility.Visible;
                return;
            }

            var name = (sender as ListBox)?.SelectedItem?.ToString();

            AddFriendButtonGray.Visibility = string.IsNullOrWhiteSpace(name) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AllUsers_OnLostFocus(object sender, RoutedEventArgs e)
        {
            AddFriendButtonGray.Visibility = Visibility.Visible;
        }

        private void AllUsers_OnGotFocus(object sender, RoutedEventArgs e)
        {
            AllUsers_OnSelectionChanged(sender, null);
        }
        #endregion
    }
}
