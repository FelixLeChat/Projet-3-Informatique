using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Model.ViewModel;
using FrontEnd.StateMachine.Core;
using EventManager = FrontEnd.Core.EventManager;


namespace FrontEnd.StateMachine.State
{
    class MatchMakingSearchingState : IState
    {
        // Little hack to close the corresponding popup
        public static MatchMakingViewModel Popup { get; set; }

        // An interested host has appear 
        private readonly ManualResetEventSlim _contactedByHostLock = new ManualResetEventSlim();
        // The host have confirmed he had created the game
        private readonly ManualResetEventSlim _confirmedByHostLock = new ManualResetEventSlim();

        private CancellationTokenSource _source;

        private const int _minTimeout = 500;
        private const int _maxTimeout = 2000;
        private readonly Random _random = new Random();

        private Guid _currentSyncId { get; set; }
        private string _gameIdToJoin { get; set; }

        public void EnterState()
        {
            _contactedByHostLock.Reset();
            _confirmedByHostLock.Reset();
            _currentSyncId = Guid.NewGuid();
            _source = new CancellationTokenSource();

            Task.Run(() => TryJoinTask(), _source.Token);
        }

        public void ExitState()
        {
            _source.Cancel();
        }

        public void Run(double deltaTime)
        {

        }

        public void Notice(IEvent triggeredEvent)
        {
            var syncEvent = triggeredEvent as MatchMakingSyncEvent;
            if (syncEvent != null)
            {
                if (syncEvent.SyncId == _currentSyncId && syncEvent.CurrentSyncStep == MatchMakingSyncEvent.SyncStep.HostWantTargetPlayer)
                {
                    Console.WriteLine("A potential host had show interest!");
                    _contactedByHostLock.Set();
                }
            }

            var gameInvitation = triggeredEvent as MatchMakingGameInvitation;
            if (gameInvitation != null)
            {
                if (gameInvitation.SyncId == _currentSyncId)
                {
                    Console.WriteLine("Game Invitation Received!");
                    Debug.Assert(!string.IsNullOrWhiteSpace(gameInvitation.GameId), "The game invitation contains an empty id");
                    _gameIdToJoin = gameInvitation.GameId;
                    _confirmedByHostLock.Set();
                }
            }
        }

        private void TryJoinTask()
        {
            if (!TryJoin())
            {
                SwitchToTryHost();
            }
        }

        private bool TryJoin()
        {
            // Send searching event
            var searchingEvent = new MatchMakingSearchingEvent(_currentSyncId);
            EventManager.Instance.Notice(searchingEvent);

            var timeout = _random.Next(_maxTimeout) + _minTimeout;
            //#1 : Wait to receive event
            if (_contactedByHostLock.Wait(timeout) && !_source.IsCancellationRequested)
            {
                // Send searching event
                Console.WriteLine("Telling host we will wait a little bit!");
                var acceptHostInterest = new MatchMakingSyncEvent(_currentSyncId, MatchMakingSyncEvent.SyncStep.PlayerWillWaitForHostInvitation);
                EventManager.Instance.Notice(acceptHostInterest);

                // Try get confirmation
                if (_confirmedByHostLock.Wait(_maxTimeout) && !_source.IsCancellationRequested)
                {
                    JoinGame(_gameIdToJoin);
                    return true;
                }
                else
                {
                    Console.WriteLine("No potential host did not sent the invitation in time");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("No host found");
                return false;
            }
        }

        private void SwitchToTryHost()
        {
            if (!_source.IsCancellationRequested)
            {
                Console.WriteLine("Switching to Host State");
                EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.MatchMakingHosting });
            }
        }


        // Join Game
        private void JoinGame(string gameId)
        {
            Application.Current.Dispatcher.Invoke(() => Popup?.CloseAction());
            Console.WriteLine($"Joining game: {gameId}");


            EventManager.Instance.Interrupt(new ChangeStateEvent() { NextState = Enums.States.OnlineGame });
            var joinEvent = new JoinOnlineGameRequestEvent()
            {
                HashId = gameId,
                IsPrivate = false,
                Name = ""
            };

            EventManager.Instance.Notice(joinEvent);
        }
    }
}
