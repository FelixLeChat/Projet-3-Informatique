using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using FrontEnd.AsyncLoading;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.UserControl;
using FrontEnd.UserControl.AchievementControl;
using FrontEnd.UserControl.Loading;
using FrontEnd.ViewModel;
using FrontEndAccess.APIAccess;
using FrontEndAccess.Ping;
using FrontEndAccess.WebsocketAccess;

namespace FrontEnd
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static MainWindow Instance;
        public static ChatMenu Chatmenu;
        private static ChatWindow _chatWindow;
        public static AchievementPanel Achievement;
        private static bool _chatInWindow;
        private bool _panelLoading;
        private string _panelMainMessage = "";
        private string _panelSubMessage = "Veuillez patienter un peu!";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Instance = this;

            try
            {
                SetupNetworkConnection();
            }
            catch (Exception e)
            {
                MessageHelper.ShowError("Un petit pépin s'est produit", "La connexion au serveur est interrompue", e);
            }

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void SetupNetworkConnection()
        {
            // Setup Access Endpoint
            var url = "ec2-52-90-46-132.compute-1.amazonaws.com";

            // Debug
            //var url = "localhost:51216/";

            var httpEndPoint = $@"http://{url}";
            //var httpEndPoint = "http://localhost:51216";
            var chatEndpoint = $@"ws://{url}/Websocket/WsChatHandler.ashx";
            var sessionEventEndpoint = $@"ws://{url}/Websocket/WsSessionEventHandler.ashx";
            var connexionEndpoint = $@"ws://{url}/Websocket/WsConnexionHandler.ashx";

            // Websocket endpoint
            WebsocketChatAccess.Instance = new WebsocketChatAccess(chatEndpoint);
            SessionEventWebsocketAccess.Initialize(sessionEventEndpoint);
            ConnexionWebsocketAccess.Instance = new ConnexionWebsocketAccess(connexionEndpoint);

            // Http endpoint
            UserAccess.Instance = new UserAccess(httpEndPoint);
            ProfileAccess.Instance = new ProfileAccess(httpEndPoint);
            FriendAccess.Instance = new FriendAccess(httpEndPoint);
            GameAccess.Instance = new GameAccess(httpEndPoint);
            PingAccess.Instance = new PingAccess(httpEndPoint);
            LeaderboardAccess.Instance = new LeaderboardAccess(httpEndPoint);
            ZoneAccess.Instance = new ZoneAccess(httpEndPoint);
            DailyAccess.Instance = new DailyAccess(httpEndPoint);

            // Setup Chat
            Chatmenu = new ChatMenu();
            ChatPanel.Content = Chatmenu;
            ChatPanel.Visibility = Visibility.Hidden;

            // Setup Achievement panel
            Achievement = new AchievementPanel();
            AchievementPanel.Content = Achievement;
            Achievement.HideAchievement();

            // Setup Message Panel
            var defaultMessage = new MessagePresenter();
            SetMessage(defaultMessage);
            defaultMessage.IsVisible = false;
        }

        public void SwitchScreen(System.Windows.Controls.UserControl nextWindow)
        {
            MainPanel.Content = nextWindow;
            ChatPanel.Visibility = User.Instance.IsConnected ? Visibility.Visible : Visibility.Hidden;
        }

        public void SwitchTitle(string title)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Title = "Princess Love Balls - " + title;
            }));
        }


        #region Message Bar

        private static readonly Timer MessageTimer = new Timer()
        {
            AutoReset = false
        }; 

        public void SetMessage(MessagePresenter message, double timeoutInMs = 5000)
        {
            MessageTimer.Stop();

            message.IsVisible = true;
            MessageControl.DataContext = message;

            MessageTimer.Interval = timeoutInMs;
            MessageTimer.Elapsed +=
                (sender, args) =>
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => message.IsVisible = false));
                };
            MessageTimer.Start();
        }

        #endregion

        #region Circular loading bar
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether [panel loading].
        /// </summary>
        /// <value>
        /// <c>true</c> if [panel loading]; otherwise, <c>false</c>.
        /// </value>
        public bool PanelLoading
        {
            get
            {
                return _panelLoading;
            }
            set
            {
                _panelLoading = value;
                RaisePropertyChanged("PanelLoading");
            }
        }

        /// <summary>
        /// Gets or sets the panel main message.
        /// </summary>
        /// <value>The panel main message.</value>
        public string PanelMainMessage
        {
            get
            {
                return _panelMainMessage;
            }
            set
            {
                _panelMainMessage = value;
                RaisePropertyChanged("PanelMainMessage");
            }
        }

        /// <summary>
        /// Gets or sets the panel sub message.
        /// </summary>
        /// <value>The panel sub message.</value>
        public string PanelSubMessage
        {
            get
            {
                return _panelSubMessage;
            }
            set
            {
                _panelSubMessage = value;
                RaisePropertyChanged("PanelSubMessage");
            }
        }

        /// <summary>
        /// Gets the panel close command.
        /// </summary>
        public ICommand PanelCloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    // Your code here.
                    // You may want to terminate the running thread etc.
                    PanelLoading = false;
                });
            }
        }

        /// <summary>
        /// Gets the show panel command.
        /// </summary>
        public ICommand ShowPanelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    PanelLoading = true;
                });
            }
        }

        /// <summary>
        /// Gets the hide panel command.
        /// </summary>
        public ICommand HidePanelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    PanelLoading = false;
                });
            }
        }

        /// <summary>
        /// Gets the change sub message command.
        /// </summary>
        public ICommand ChangeSubMessageCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    PanelSubMessage = string.Format("Message: {0}", DateTime.Now);
                });
            }
        }

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">UserName of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Achievement bar
        #endregion

        #region Chat Window
        public void ExtractChat()
        {
            _chatWindow = new ChatWindow(Chatmenu);

            _chatWindow.Show();
            _chatWindow.Activate();
            ChatPanel.Content = null;
            _chatInWindow = true;
        }

        public void InsertChat(bool close = true)
        {
            if (close)
                _chatWindow.Close();

            ChatPanel.Content = Chatmenu;
            _chatWindow.Visibility = Visibility.Collapsed;
            _chatInWindow = false;
        }

        public void HandleChatOnLogout()
        {
            if (_chatInWindow)
                InsertChat();
        }
        #endregion

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _chatWindow?.Close();

            if(User.Instance.IsConnected)
                Load.LoadOnLogout();
        }

        private bool IsFullScreen { get; set; }
        private bool AltKeyPressed { get; set; }
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (AltKeyPressed)
            {
                AltKeyPressed = false;
                if (e.SystemKey == Key.Enter)
                {
                    WindowState = WindowState.Normal;
                    ResizeMode = IsFullScreen ? ResizeMode.NoResize : ResizeMode.CanResize;
                    WindowStyle = IsFullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Maximized;
                    IsFullScreen = !IsFullScreen;
                }
            }

            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            {
                AltKeyPressed = true;
            }
        }
    }
}
