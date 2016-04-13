using System;
using System.Diagnostics;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Model.ViewModel;
using FrontEnd.Player;
using FrontEnd.StateMachine.Core;
using FrontEnd.UserControl.Popup;
using FrontEndAccess.WebsocketAccess;

namespace FrontEnd.Game.Handler
{
    public class GeneralOnlineEventHandler
    {
        private static readonly object _instanceLock = new object();

        private static GeneralOnlineEventHandler _instance = new GeneralOnlineEventHandler();
        public static GeneralOnlineEventHandler Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new GeneralOnlineEventHandler();
                    }
                }
                return _instance;
            }
        }

        private GeneralOnlineEventHandler()
        {
        }

        public static void Initiate()
        {
            EventManager.Instance.Subscribe(Instance.HandleOnlineEvent);
        }

        public static void Stop()
        {
            EventManager.Instance.Unsubscribe(Instance.HandleOnlineEvent);
        }

        /// <summary>
        /// Listen to local event and send it to the server if needed
        /// (ex: listen to game invitation)
        /// </summary>
        public void HandleOnlineEvent(IEvent localEvent)
        {
            var sessionEvent = localEvent as AbstractSessionEvent;
            if (sessionEvent != null && !sessionEvent.SendToServer)
            {
                var invitation = sessionEvent as InvitationEvent;

                // Can't receive invitation if already in game
                if (invitation != null)
                {
                    if (invitation.InvitedPlayerId == Profile.Instance.CurrentProfile.UserHashId && !StateManager.Instance.IsGameRunning())
                    {
                        try
                        {
                            var context = new GameInvitationViewModel(invitation);
                            var invitationPopup = new GameInvitation(context);
                            invitationPopup.ShowDialog();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Received a game that does not exist");
                        }
                    }

                }
            }
        }
    }
}