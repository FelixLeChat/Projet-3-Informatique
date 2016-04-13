using System.ComponentModel;
using System.Runtime.CompilerServices;
using FrontEnd.Annotations;

namespace FrontEnd.Game
{
    /// <summary>
    /// Use as DataContext in a view: Basic configuration for solo game
    /// </summary>
    public class BasicOnlineGameConfig : INotifyPropertyChanged
    {
        /// <summary>
        /// UserName of the online game
        /// </summary>
        private string _gameName;
        public string GameName
        {
            get { return _gameName; }
            set
            {
                if (_gameName != value)
                {
                    _gameName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// If the game is private it will need a password
        /// </summary>
        private bool _isPrivate;
        public bool IsPrivate
        {
            get { return _isPrivate; }
            set
            {
                if (_isPrivate != value)
                {
                    _isPrivate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Password if the game is private
        /// </summary>
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// If it is a quickgame or a campaing
        /// </summary>
        private ZoneConfig _zoneConfig;
        public ZoneConfig ZoneConfig
        {
            get { return _zoneConfig; }
            set
            {
                if (_zoneConfig != value)
                {
                    _zoneConfig = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// If the mode is coop or competitive
        /// </summary>
        private MultiMode _multiMode;
        public MultiMode MultiMode
        {
            get { return _multiMode; }
            set
            {
                if (_multiMode != value)
                {
                    _multiMode = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Number of player wanted
        /// </summary>
        private int _playerCount;
        public int PlayerCount
        {
            get { return _playerCount; }
            set
            {
                if (_playerCount != value)
                {
                    _playerCount = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}