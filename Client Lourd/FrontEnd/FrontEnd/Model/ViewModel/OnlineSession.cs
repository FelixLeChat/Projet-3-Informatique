using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Helper;
using FrontEnd.Model.ViewModel;
using FrontEnd.ModelConverter;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.UserControl.Popup;
using FrontEnd.ViewModel.Base;
using FrontEndAccess.APIAccess;
using Models.Database;

namespace FrontEnd.Game
{
    /// <summary>
    /// View model that contains the information about the Online Session
    /// Presently used with the WaitingRoom view
    /// </summary>
    public class OnlineSession : ObservableObject
    {
        public string SessionName { get; set; }
        public string SessionId { get; set; }
        public string Password { get; set; }

        public bool IsHost { get; set; }

        public ObservableCollection<SessionParticipant> Participants { get; } =
            new ObservableCollection<SessionParticipant>();

        // Due to network we may receive the player is ready before we know he his in the room
        private List<string> _unhandleReadyPlayer = new List<string>();

        // Todo: change for Command Parameter
        public SessionParticipant SelectedParticipant { get; set; }

        /// <summary>
        /// Update the participants list: do not set the state of this participant (this is from the events)
        /// </summary>
        /// <param name="model"></param>
        public void UpdateParticipantsList(GameModel model)
        {
            // Remove exited participant
            foreach (var presentParticipants in Participants.ToList())
            {
                if (!model.ParticipantsHashId.Contains(presentParticipants.HashId))
                {
                    Participants.Remove(presentParticipants);
                }
            }

            bool addNewParticipant = false;
            foreach (var playerId in model.ParticipantsHashId)
            {
                /// Todo: check if can use linq instead
                bool isPresent = false;
                foreach (var presentParticipants in Participants)
                {
                    if (playerId == presentParticipants.HashId)
                    {
                        isPresent = true;
                        break;
                    }
                }
                if (!isPresent)
                {
                    var newPlayerModel = ProfileAccess.Instance.GetUserInfoPlease(playerId);
                    var newPlayer = ProfileModelConverter.ConvertToParticipant(newPlayerModel);
                    if (_unhandleReadyPlayer.Contains(newPlayer.HashId))
                    {
                        newPlayer.CurrentState = SessionParticipant.ParticipantState.ReadyToStart;
                        _unhandleReadyPlayer.Remove(newPlayer.HashId);
                    }
                    Participants.Add(newPlayer);
                    addNewParticipant = true;
                }
            }
            if (addNewParticipant)
            {
                SendInfoForNewPlayer();
            }

        }

        private void HandleEvent(IEvent triggeredEvent)
        {
            var playerIsReady = triggeredEvent as PlayerReadyEvent;
            if (playerIsReady != null)
            {
                bool wasHandle = false;
                foreach (var presentParticipants in Participants)
                {
                    if (playerIsReady.PlayerId == presentParticipants.HashId)
                    {
                        presentParticipants.CurrentState = SessionParticipant.ParticipantState.ReadyToStart;
                        wasHandle = true;
                        break;
                    }
                }

                // We receive the IsReady before we know the player is in the room
                if (!wasHandle)
                {
                    _unhandleReadyPlayer.Add(playerIsReady.PlayerId);
                }
            }
        }

        public ICommand KickPlayerCommand => new DelegateCommand(KickPlayer);

        private void KickPlayer()
        {
            if (SelectedParticipant == null)
            {
                MessageHelper.ShowMessage("Votre puissant serviteur ne comprend pas votre ordre", "Vous devez lui pointer un causeur de trouble pour qu'il puisse l'expulser");
                return;
            }
            if (SelectedParticipant.HashId == Profile.Instance.CurrentProfile.UserHashId)
            {
                MessageHelper.ShowMessage("Votre puissant serviteur ne peut vous satisfaire", "D'après son contrat, il n'a pas le droit d'expulser la princesse");
                return;
            }

            EventManager.Instance.Notice(new KickPlayerEvent() { KickedPlayerId = SelectedParticipant.HashId });
        }

        public ICommand StartGameCommand => new DelegateCommand(StartGame);

        private void StartGame()
        {
            Debug.Assert(IsHost, "The not host should not see the start button");

            // Todo: Not thread safe
            foreach (var player in Participants)
            {
                if (player.CurrentState != SessionParticipant.ParticipantState.ReadyToStart)
                {
                    MessageHelper.ShowMessage("Patiente!", "Attendez que vos invités finissent de mettre leur diadème de danse");
                    return;
                }
            }

            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.LaunchGame });
        }


        public ICommand InviteFriendCommand => new DelegateCommand(InviteFriend);

        private void InviteFriend()
        {
            var friends = FriendInvitationHelper.GetFriendInvitation();
            friends.ForEach(x =>
            {
                x.GameId = SessionId;
                x.Password = Password;
            });

            var participant = ObservableCollectionConverter.ConvertObservableCollection<SessionParticipant>(Participants);

            friends = friends.Where(x => !participant.Any(c => x.Player.HashId == c.Player.HashId)).ToList();

            if (friends.Any())
            {
                var popup = new InviteFriendPopup(new InviteFriendsViewModel(friends));
                popup.ShowDialog();
            }
            else
            {
                MessageHelper.ShowMessage("Désolé tu n'as pas d'autre ami", "Un truc pour se faire des amis: aller voir quelqu'un et devenir son ami!");
            }
        }

        /// <summary>
        /// When a new player join the game he may have miss some important events
        /// </summary>
        private void SendInfoForNewPlayer()
        {
            //Todo: Send Player ready event
            EventManager.Instance.Notice(new PlayerReadyEvent()
            {
                PlayerId = Profile.Instance.CurrentProfile.UserHashId
            });
        }

        #region Singleton Method
        private static OnlineSession _instance;

        public static OnlineSession Instance
        {
            get { return _instance; }
        }

        public static void InitializeSession(GameModel model)
        {
            Debug.Assert(_instance == null, "The previous OnlineSession must be terminated before creating a new one");

            _instance = new OnlineSession();
            EventManager.Instance.Subscribe(Instance.HandleEvent);
            _instance.SessionId = model.HashId;
            _instance.Password = model.Password;
            _instance.SessionName = model.Name;
            _instance.UpdateParticipantsList(model);
            _instance.IsHost = model.HostHashId == Profile.Instance.CurrentProfile.UserHashId;
        }
        #endregion

        public static void TerminateSession()
        {
            try
            {
                if (_instance != null)
                {
                    EventManager.Instance.Unsubscribe(Instance.HandleEvent);
                    if (ConnectionHelper.IsConnected)
                    {
                        //if (_instance.IsHost)
                        //{
                        //    EventManager.Instance.Interrupt(new TerminateSessionEvent()
                        //    {
                        //        Cause = terminationCause
                        //    });
                        //}
                        GameAccess.Instance.QuitGame(Instance.SessionId);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while terminating session\n{0}", e);
            }
            finally
            {
                _instance = null;
            }

        }

    }
}