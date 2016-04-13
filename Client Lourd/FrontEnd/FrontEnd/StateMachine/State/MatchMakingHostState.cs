using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game.Config.Helper;
using FrontEnd.Model.ViewModel;
using FrontEnd.Player;
using FrontEnd.StateMachine.Core;
using FrontEndAccess.APIAccess;
using Models.Database;
using EventManager = FrontEnd.Core.EventManager;


namespace FrontEnd.StateMachine.State
{
    class MatchMakingHostState : IState
    {
        // Little hack to close the corresponding popup
        public static MatchMakingViewModel Popup { get; set; }

        // When a client that searching contacted this host
        private readonly ManualResetEventSlim _contactedByClientLock = new ManualResetEventSlim();
        // When a client confirmed that he is waiting for the game creation
        private readonly ManualResetEventSlim _confirmedByClientLock = new ManualResetEventSlim();

        private CancellationTokenSource _source;

        private const int _minTimeout = 500;
        private const int _maxTimeout = 2000;
        private readonly Random _random = new Random();

        private Guid _currentSyncId { get; set; }
        private string _potentialPlayerId { get; set; }

        public void EnterState()
        {
            _contactedByClientLock.Reset();
            _confirmedByClientLock.Reset();
            _source = new CancellationTokenSource();

            Task.Run(() => TryHostTask(), _source.Token);
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
            var searchingEvent = triggeredEvent as MatchMakingSearchingEvent;
            if (searchingEvent != null && !_contactedByClientLock.IsSet)
            {
                _currentSyncId = searchingEvent.SyncId;
                _potentialPlayerId = searchingEvent.PlayerId;
                _contactedByClientLock.Set();
            }

            var confirmationEvent = triggeredEvent as MatchMakingSyncEvent;
            if (confirmationEvent != null)
            {
                if (confirmationEvent.SyncId == _currentSyncId && confirmationEvent.CurrentSyncStep == MatchMakingSyncEvent.SyncStep.PlayerWillWaitForHostInvitation)
                {
                    Console.WriteLine("A potential searcher had show interest!");
                    _confirmedByClientLock.Set();
                }
            }
        }

        private void TryHostTask()
        {
            string gameId;
            if (TryHost(out gameId))
            {
                JoinGame(gameId);
            }
            else
            {
                SwitchToTryJoin();
            }
        }

        private bool TryHost(out string gameId)
        {
            gameId = null;
            var timeout = _random.Next(_maxTimeout) + _minTimeout;
            //#1 : Wait to receive event
            if (_contactedByClientLock.Wait(timeout) && !_source.IsCancellationRequested)
            {
                Console.WriteLine("Showing interest in searcher!");
                var showInterestToClient = new MatchMakingSyncEvent(_currentSyncId, MatchMakingSyncEvent.SyncStep.HostWantTargetPlayer);
                EventManager.Instance.Notice(showInterestToClient);

                // Try get confirmation
                if (_confirmedByClientLock.Wait(_maxTimeout) && !_source.IsCancellationRequested)
                {
                    // Create game + send invitation
                    var createdGame = CreateGame();
                    Console.WriteLine("Sending game invitation!");
                    var invitationEvent = new MatchMakingGameInvitation(_currentSyncId, createdGame.HashId);
                    EventManager.Instance.Notice(invitationEvent);
                    gameId = createdGame.HashId;
                    return true;
                }
                else
                {
                    Console.WriteLine("The potential client did not confirmed!");
                    return false;
                }

            }
            Console.WriteLine("No client found!");
            return false;
        }

        private void SwitchToTryJoin()
        {
            if (!_source.IsCancellationRequested)
            {
                Console.WriteLine("Switching to Search State");
                EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.MatchMakingSearching });
            }
        }

        private void JoinGame(string gameId)
        {
            Application.Current.Dispatcher.Invoke(() => Popup?.CloseAction());
            Console.WriteLine($"Joining game: {gameId}");
            EventManager.Instance.Interrupt(new ChangeStateEvent() { NextState = Enums.States.OnlineGame });
            var joinEvent = new HostJoiningCreatedGameEvent()
            {
                HashId = gameId
            };

            EventManager.Instance.Notice(joinEvent);
        }

        // Create a Game (on the server) 
        private GameModel CreateGame()
        {
            int minLevel = Math.Min(Profile.Instance.CurrentProfile.Level,
                                    ProfileAccess.Instance.GetUserInfoPlease(_potentialPlayerId).Level);
            var game = MatchMakingDefaultGame.GetModelForLevel(minLevel);
            var createdGame = GameAccess.Instance.CreateGame(game);
            return createdGame;
        }
    }
}
