using System.ComponentModel;
using System.Runtime.CompilerServices;
using FrontEnd.Annotations;

namespace FrontEnd.Game
{
    /// <summary>
    /// Use as DataContext in a view: Basic configuration for solo game
    /// </summary>
    public class BasicOfflineGameConfig : INotifyPropertyChanged
    {

        private PlayerCountMode _playerCount;
        public PlayerCountMode PlayerCount
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

        private PlayerType _playerType;
        public PlayerType PlayerTypes
        {
            get { return _playerType; }
            set
            {
                if (_playerType != value)
                {
                    _playerType = value;
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