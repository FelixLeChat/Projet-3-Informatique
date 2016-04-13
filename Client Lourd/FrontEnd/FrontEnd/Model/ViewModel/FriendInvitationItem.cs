using System;
using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game;
using FrontEnd.Player;
using FrontEnd.ViewModel.Base;
using FrontEndAccess.APIAccess;
using Models;
using System.Timers;

namespace FrontEnd.Model.ViewModel
{
    /// <summary>
    /// Represent a participant in a online game session
    /// </summary>
    public class FriendInvitationItem : ObservableObject, IDisposable
    {
        private Timer tmr;
        public FriendInvitationItem()
        {
            tmr = new Timer();       // Doesn't require any args
            tmr.Interval = 500;
            tmr.Elapsed += delegate
            {
                CheckOnlineStateTask();
            };    // Uses an event instead of a delegate
            tmr.Start();

        }

        public void Dispose()
        {
            tmr.Stop();
        }

        private void CheckOnlineStateTask()
        {
            if (ProfileAccess.Instance.GetIsConnected(Player.HashId))
            {
                CurrentState = State.Online;
            }
            else
            {
                CurrentState = State.Offline;
            }
        }

        public enum State
        {
            Offline = 0,
            Online,
        }

        public Player Player { get; set; }

        public string GameId { get; set; }
        public string Password { get; set; }

        private State _currentState;
        public State CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                RaisePropertyChangedEvent(nameof(CurrentState));
                InvitePossible = CurrentState == State.Online && !InvitationWasSent;

            }
        }

        private bool _invitationWasSent = false;

        public bool InvitationWasSent
        {
            get { return _invitationWasSent; }
            set
            {
                _invitationWasSent = value;
                RaisePropertyChangedEvent(nameof(InvitationWasSent));
                InvitePossible = CurrentState == State.Online && !InvitationWasSent;
            }
        }

        private bool _invitationPossible;
        public bool InvitePossible {
            get { return _invitationPossible; }
            set {
                _invitationPossible = value;
                RaisePropertyChangedEvent(nameof(InvitePossible));
            }
        }


        public ICommand InviteCommand => new DelegateCommand(Invite);

        private void Invite()
        {
            if (!InvitationWasSent)
            {
                var invitation = new InvitationEvent()
                {
                    HashId = GameId,
                    Password = Password,
                    InvitedPlayerId = Player.HashId,
                    InviterId = Profile.Instance.CurrentProfile.UserHashId
                };
                EventManager.Instance.Notice(invitation);

                InvitationWasSent = true;
            }


        }
    }
}