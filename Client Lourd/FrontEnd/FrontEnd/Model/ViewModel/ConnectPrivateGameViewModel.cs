using System;
using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game;
using FrontEnd.ViewModel.Base;
using Models.Database;

namespace FrontEnd.Model.ViewModel
{
    /// <summary>
    /// View model for the popup to ask about to reconnect from a disconnected game (on login)
    /// </summary>
    public class ConnectPrivateGameViewModel : ObservableObject
    {
        // Need to bind in the view
        public Action CloseAction { get; set; }

        private string _gameId;

        public string Password { get; set; }  = "";
        public string GameName { get; set; }



        public ConnectPrivateGameViewModel(string gameId, string gameName)
        {
            _gameId = gameId;
            GameName = gameName;
        }

        public ICommand JoinCommand => new DelegateCommand(Join);

        private void Join()
        {
            EventManager.Instance.Notice(new JoinOnlineGameRequestEvent() {HashId = _gameId, IsPrivate = true, Password = Password});
            CloseAction();
        }

        public ICommand CancelCommand => new DelegateCommand(Command);

        private void Command()
        {
            CloseAction();
        }
    }
}