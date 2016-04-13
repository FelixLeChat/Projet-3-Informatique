using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using FrontEnd.Player;
using FrontEnd.Stats;
using FrontEndAccess;
using FrontEndAccess.WebsocketAccess;
using Models;
using Models.Communication;
using Newtonsoft.Json;

namespace FrontEnd.UserControl
{
    public partial class ChatMenu
    {
        private static WebsocketChatAccess _chatAccess;
        private static ChatMenu _instance;
        private static User _user;
        private string _currentChatId;
        public const string DefaultCanal = "General";
        private readonly Dictionary<string, string> _chatContent = new Dictionary<string, string>();
        private readonly Dictionary<string, TabItem> _tabItems = new Dictionary<string, TabItem>();
        private static readonly Dictionary<string, string> CanalHashs = new Dictionary<string, string>();
        private bool IsExtracted = false;

        public ChatMenu()
        {
            InitializeComponent();
            _instance = this;
            _user = User.Instance;

            // Init Websocket
            _chatAccess = WebsocketChatAccess.Instance;
            // Add event for chat update notification
            _chatAccess.AddEvent((sender, args) => { ReceiveMessage(args.Data); });
            // Set current chat to general
            //AddNewTab(DefaultCanal);
        }

        /// <summary>
        /// Send message for the specified chat Id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _chatAccess.SendMessage(TextBox.Text, _currentChatId, _user.UserToken);
            TextBox.Focus();
            TextBox.Text = "";

            // Daily chat
            DailyManager.AchieveDaily(EnumsModel.DailyType.ChatOnce);
        }

        /// <summary>
        /// Check key input in chat box to send the data to the server on a enter key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter || !(sender is TextBox))
                return;
            _chatAccess.SendMessage(((TextBox)sender).Text, _currentChatId, _user.UserToken);
            TextBox.Text = "";

            // Daily chat
            DailyManager.AchieveDaily(EnumsModel.DailyType.ChatOnce);
        }

        /// <summary>
        /// Decode received message and dispatch it
        /// </summary>
        /// <param name="text"></param>
        private void ReceiveMessage(string text)
        {
            // to be executed in any threads
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                var decodedError = JsonConvert.DeserializeObject<ErrorMessage>(text);
                var decodedMessage = JsonConvert.DeserializeObject<WebsocketChatMessage>(text);

                if (!string.IsNullOrWhiteSpace(decodedError.Error))
                {
                    MessageBox.Show(decodedError.Error);
                    return;
                }
                // UpdateWindow chat content and write in chat if the current selected chat is the one 
                // the message come from
                if (!string.IsNullOrWhiteSpace(decodedMessage.Message) && _chatContent.ContainsKey(decodedMessage.CanalId))
                {

                    _chatContent[decodedMessage.CanalId] += decodedMessage.Message + Environment.NewLine;
                
                    if(_currentChatId == decodedMessage.CanalId && _chatContent.ContainsKey(_currentChatId))
                        WriteInChat(_chatContent[_currentChatId]);
                    else if (_chatContent.ContainsKey(_currentChatId))
                    {
                        // color the tab of where a message has been received
                        foreach (var item in _tabItems)
                        {
                            try
                            {
                                var hash = CanalHashs[item.Key];
                                if (hash == decodedMessage.CanalId)
                                    item.Value.Foreground = new SolidColorBrush(Colors.Red);
                            }
                            catch
                            {
                                
                            }
                        }
                    }
                }

                // UpdateWindow the canal list and change canal
                if (!string.IsNullOrWhiteSpace(decodedMessage.CanalName) &&
                    !string.IsNullOrWhiteSpace(decodedMessage.CanalId))
                {
                    if(!CanalHashs.ContainsKey(decodedMessage.CanalName))
                        CanalHashs.Add(decodedMessage.CanalName, decodedMessage.CanalId);

                    AddNewTab(decodedMessage.CanalName);
                }
            }));
        }

        /// <summary>
        /// Write the specified text in the Chat (enable any thread to do so)
        /// </summary>
        /// <param name="text"></param>
        private static void WriteInChat(string text)
        {
            if (text == "") return;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                _instance.ChatTextBlock.Text = text;
                _instance.ChatTextScroll.ScrollToBottom();
            }));
        }

        #region Tab Management
        /// <summary>
        /// Create a new tab and add it to the tabviewer
        /// </summary>
        /// <param name="name"></param>
        public static void AddNewTab(string name)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                // If name already exist
                TabItem tab = null;
                var i = 0;
                var index = 0;
                foreach (var tabItem in _instance._tabItems)
                {
                    if (tabItem.Key.ToLower() == name.ToLower())
                    {
                        tab = tabItem.Value;
                        index = i;
                    }
                    i++;
                }

                if (tab != null)
                {
                    _instance.ChatTabs.SelectedIndex = index;
                    tab.Visibility = Visibility.Visible;
                    ChangeCanal(name);
                }
                else
                {
                    var idx = _instance.ChatTabs.Items.Count;
                    try
                    {
                        var tabItem = new TabItem {Header = name};
                        tabItem.PreviewMouseDown += TabItemOnMouseDown;

                        if (name != DefaultCanal)
                            tabItem.MouseRightButtonUp += TabItemOnMouseRightButtonUp;

                        _instance.ChatTabs.Items.Insert(idx - 1, tabItem);
                        _instance._tabItems.Add(name, tabItem);
                        _instance.ChatTabs.SelectedIndex = idx - 1;
                        ChangeCanal(name);
                    }
                    catch
                    {
                        // catch any stupid exceptions!!!
                    }
                }
            }));
        }

        public static void StartChatWithFriend(string friendHashId, string friendName)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
            {
                // If name already exist
                TabItem tab = null;
                var i = 0;
                var index = 0;
                foreach (var tabItem in _instance._tabItems)
                {
                    if (tabItem.Key.ToLower() == friendName.ToLower())
                    {
                        tab = tabItem.Value;
                        index = i;
                    }
                    i++;
                }

                if (tab != null)
                {
                    _instance.ChatTabs.SelectedIndex = index;
                    tab.Visibility = Visibility.Visible;
                    ChangeCanal(friendName);
                }
                else
                {
                    WebsocketChatAccess.Instance.StartChatWithFriend(friendHashId, friendName, UserToken.Token);
                }
            }));
        }

        private static void TabItemOnMouseRightButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            // Hide tab
            ((TabItem) sender).Visibility = Visibility.Collapsed;

            // Select default canal
            _instance.ChatTabs.SelectedIndex = 0;
            ChangeCanal(DefaultCanal);
            
        }

        /// <summary>
        /// Mouse down on tab item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private static void TabItemOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var item = sender as TabItem;
            if (item == null) return;

            var tabItem = item;
            var headerName = tabItem.Header.ToString();
            ChangeCanal(headerName);
            tabItem.Foreground = new SolidColorBrush(Colors.Black);
            _instance.ChatTextScroll.ScrollToBottom();
        }

        /// <summary>
        /// Change the tab to the specified canal name
        /// </summary>
        /// <param name="name"></param>
        private static void ChangeCanal(string name)
        {
            // Change canal
            _instance._currentChatId = CanalHashs[name];
            if (!_instance._chatContent.ContainsKey(_instance._currentChatId))
                _instance._chatContent.Add(_instance._currentChatId, "");
            _instance.ChatTextBlock.Text = _instance._chatContent[_instance._currentChatId];
        }

        private void TabItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var inputDialog = new ChatCanalInputWindow("Entrez le nom du nouveau canal:");
            if (inputDialog.ShowDialog() == true)
            {
                var result = inputDialog.Answer;
                // Send message to create new canal
                _chatAccess.CreateNewCanal(result, _user.UserToken);

                // Daily create canal
                DailyManager.AchieveDaily(EnumsModel.DailyType.CreateCanal);
            }
        }
        #endregion

        private void MinimizeChat_Click(object sender, RoutedEventArgs e)
        {
            ChatBorder.Visibility = Visibility.Collapsed;
            MaximizeChatBorder.Visibility = Visibility.Visible;
        }

        private void MaximizeChat_Click(object sender, RoutedEventArgs e)
        {
            ChatBorder.Visibility = Visibility.Visible;
            MaximizeChatBorder.Visibility = Visibility.Collapsed;
        }

        private void ExtractChat_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ExtractChat();
            InsertChat.Visibility = Visibility.Visible;
            ExtractChat.Visibility = Visibility.Collapsed;
            GridSplitter.HorizontalAlignment =HorizontalAlignment.Right;
            GridSplitter.VerticalAlignment =VerticalAlignment.Bottom;
            MaximizeChatBorder.Visibility = Visibility.Collapsed;
            MinimizeChat.Visibility = Visibility.Collapsed;
            IsExtracted = true;
            ChatBorder.Opacity = 1.0;

        }

        private void InsertChat_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.InsertChat();
            InsertChat.Visibility = Visibility.Collapsed;
            ExtractChat.Visibility = Visibility.Visible;
            GridSplitter.HorizontalAlignment = HorizontalAlignment.Left;
            GridSplitter.VerticalAlignment = VerticalAlignment.Top;
            MinimizeChat.Visibility = Visibility.Visible;
            IsExtracted = false;
            ChatBorder.Opacity = 0.8;
        }

        /// <summary>
        /// Handle for when the user click on the X of the chat window
        /// </summary>
        public void OnWindowClosing()
        {
            MainWindow.Instance.InsertChat(false);
            InsertChat.Visibility = Visibility.Collapsed;
            ExtractChat.Visibility = Visibility.Visible;
        }

        private void GridSplitter_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            int multiplier = -1;
            if (IsExtracted)
                multiplier = 1;
            var newWidth = Width + e.HorizontalChange * multiplier;
            Width = Math.Min(Math.Max(newWidth, 300), 800);
           

            var newHeight = Height + e.VerticalChange * multiplier;
            Height = Math.Min(Math.Max(newHeight, 200), 600);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox != null && textbox.Text.Length > 255)
            {
                textbox.Text = textbox.Text.Substring(0, 255);
                textbox.CaretIndex = 255    ;
            }
        }
    }
}