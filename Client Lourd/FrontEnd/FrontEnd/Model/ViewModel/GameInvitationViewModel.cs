using System;
using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game;
using FrontEnd.ViewModel.Base;
using FrontEndAccess.APIAccess;
using Models.Database;

namespace FrontEnd.Model.ViewModel
{
    /// <summary>
    /// View model for the popup to ask about to reconnect from a disconnected game (on login)
    /// </summary>
    public class GameInvitationViewModel : ObservableObject
    {
        // Need to bind in the view
        public Action CloseAction { get; set; }

        public string Password { get; set; }

        private GameModel _model;

        private string _gameName;
        public string GameName
        {
            get { return _gameName; }
            set
            {
                _gameName = value;
                RaisePropertyChangedEvent(nameof(GameName));
            }
        }

        private string _inviterName;
        public string InviterName
        {
            get { return _inviterName; }
            set
            {
                _inviterName = value;
                RaisePropertyChangedEvent(nameof(InviterName));
            }
        }



        public GameInvitationViewModel(InvitationEvent invitation)
        {
            _model = GameAccess.Instance.GetGameInfo(invitation.HashId);
            GameName = _model.Name;
            Password = invitation.Password;
            var _player = ProfileAccess.Instance.GetUserInfoPlease(invitation.InviterId);
            InviterName = _player.Username;
        }

        public ICommand JoinCommand => new DelegateCommand(Join);

        private void Join()
        {
            // Don't join if already in game
            if (OnlineSession.Instance?.SessionId == _model.HashId)
            {
                CloseAction();
                return;
            }

            OnlineSession.TerminateSession();
            EventManager.Instance.Interrupt(new ChangeStateEvent() { NextState = Enums.States.OnlineGame });
            EventManager.Instance.Notice(new JoinOnlineGameRequestEvent() { HashId = _model.HashId, IsPrivate = _model.IsPrivate, Password = Password, Name = _model.Name });
            CloseAction();
        }

        public ICommand DeclineCommand => new DelegateCommand(Decline);

        private void Decline()
        {
            Console.WriteLine("Decline");
            CloseAction();
        }
    }
}