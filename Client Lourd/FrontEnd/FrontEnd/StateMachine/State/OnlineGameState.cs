using System;
using System.Diagnostics;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Game.Config;
using FrontEnd.Game.Wrap;
using FrontEnd.Helper;
using FrontEnd.Model.ViewModel;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.StateMachine.Core;
using FrontEnd.UserControl;
using FrontEnd.UserControl.GameControl;
using FrontEnd.UserControl.Popup;
using FrontEnd.UserControl.Winform;
using FrontEndAccess.APIAccess;
using Models;
using Models.Database;

namespace FrontEnd.StateMachine.State
{
    public class OnlineGameState : IState, IGameState
    {
        private OnlineGame _currentGame;

        private enum OnlineSubstate
        {
            OnlineMenu,
            OnlineBoard,
            GameCreation,
            WaitingRoom,
            InGame
        }

        private OnlineSubstate _currentSubState;

        public void EnterState()
        {
            EnterSubstate(OnlineSubstate.OnlineMenu);
        }

        public void ExitState()
        {
            if (_currentGame == null)
            {
                Console.WriteLine("The game should be clean up before exiting this state");
            }
        }

        /// <summary>
        /// Need to update the current game if it is running
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Run(double deltaTime)
        {
            // Handle disconnection from the Internet
            if (!ConnectionHelper.IsConnected)
            {
                if (IsGameRunning())
                {
                    EndGame(EndGameType.Disconnect);
                }
                ExitGame();
                EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
            }

            if (IsGameRunning())
            {
                _currentGame.Run(deltaTime);
            }
        }


        public void Notice(IEvent triggeredEvent)
        {
            var uiEvent = triggeredEvent as UiEvent;
            if (uiEvent != null)
            {
                HandleUiEvent(uiEvent);
            }

            var gameEnd = triggeredEvent as EndGameEvent;
            if (gameEnd != null)
            {
                Console.WriteLine("**************** Received EndGameEvent ****************");

                EndGame(gameEnd.Info);
                ExitGame();

                EnterSubstate(OnlineSubstate.OnlineBoard);
            }

            // After the user create a game in the host menu
            var createGame = triggeredEvent as CreateGameRequestEvent;
            if (createGame != null)
            {
                var config = createGame.Config as OnlineGameConfig;
                Debug.Assert(config != null, "Received a CreateGameRequestEvent with no valid OnlineGameConfig");

                var model = config.GetGameModel();
                // Create game on server
                var created = GameAccess.Instance.CreateGame(model);
                JoinWaitingRoom(created.HashId, true, model.Password);
            }

            var spectateGame = triggeredEvent as SpectateGameEvent;
            if (spectateGame != null)
            {
                GameAccess.Instance.Spectate(spectateGame.HashId);
                // Todo: Add on Spectator list on server
                JoinSession(spectateGame.HashId);
            }

            // When the player want to join a hosted game
            var joinGame = triggeredEvent as JoinOnlineGameRequestEvent;
            if (joinGame != null)
            {

                if (JoinGameHelper.IsReconnection(joinGame.HashId))
                {
                    ReconnectToGame(joinGame.HashId);
                }
                else if (JoinGameHelper.CanJoin(joinGame.HashId, true))
                {
                    try
                    {
                        // Show popup asking the password
                        if (joinGame.IsPrivate && joinGame.Password == null)
                        {
                            var context = new ConnectPrivateGameViewModel(joinGame.HashId, joinGame.Name);
                            var popup = new ConnectToPrivateGame(context);
                            popup.ShowDialog();
                        }
                        else
                        {
                            GameAccess.Instance.JoinGame(joinGame.HashId, joinGame.Password);
                            JoinWaitingRoom(joinGame.HashId);
                        }
                    }
                    catch (Exception e)
                    {
                        // Cannot join game
                        if (joinGame.IsPrivate)
                        {
                            MessageHelper.ShowError("Vous ne pouvez pas rejoindre ce bal", "Avez vous donné le bon mots de passes?", e);
                        }
                        else
                        {
                            MessageHelper.ShowError("Vous ne pouvez pas rejoindre ce bal", "Et je ne suis pas sur de comprendre pourquoi (s'il vous plait regarder votre journal de princesse)", e);
                        }
                    }
                }
            }

            var matchMaking = triggeredEvent as HostJoiningCreatedGameEvent;
            if (matchMaking != null)
            {
                JoinSession(matchMaking.HashId);
            }


            // Handle the event only if it came from the server
            var onlineSession = triggeredEvent as AbstractSessionEvent;
            if (onlineSession != null && !onlineSession.SendToServer)
            {
                HandleOnlineSessionEvent(onlineSession);
            }

            var reconnectToGame = triggeredEvent as ReconnectToGameEvent;
            if (reconnectToGame != null)
            {
                ReconnectToGame(reconnectToGame.Game.HashId);
            }
        }

        /// <summary>
        /// Join the game without calling join on the server

        /// </summary>
        /// <param name="model"></param>
        void ReconnectToGame(string gameId)
        {
            GameAccess.Instance.ReconnectGame(gameId);
            JoinSession(gameId);
        }

        /// <summary>
        ///  Will go the waiting room if the game is not started or in the game if the game is started
        /// </summary>
        /// <param name="gameId"></param>
        void JoinSession(string gameId)
        {
            var model = GameAccess.Instance.GetGameInfo(gameId);
            JoinWaitingRoom(model.HashId, model.HostHashId == Profile.Instance.CurrentProfile.UserHashId);
            if (model.State == EnumsModel.GameState.Started)
            {
                _currentGame.Start(true);
                _currentSubState = OnlineSubstate.InGame;
            }
        }

        //Todo: remove hack of is host
        /// <summary>
        /// Create the game wrapper and join the waiting room
        /// </summary>
        /// <param name="id"></param>
        private void JoinWaitingRoom(string id, bool isHost = false, string password = null)
        {
            var game = GameAccess.Instance.GetGameInfo(id);
            game.Password = password;
            // Todo: refactor game mode
            _currentGame = new OnlineGame(game.ZonesHashId.Count > 1 ? IntegratedOpenGl.Mode.ModeCampagne : IntegratedOpenGl.Mode.ModePartieRapide, game);
            _currentGame.IsHost = isHost;
            EnterSubstate(OnlineSubstate.WaitingRoom);

            // Todo: make this async
            _currentGame.Load();
        }

        private void HandleUiEvent(UiEvent uiEvent)
        {
            switch (uiEvent.Info)
            {
                case UiEvent.EventInfo.LaunchGame:
                    if (_currentGame.IsHost)
                    {
                        GameAccess.Instance.StartGame(_currentGame.Model.HashId);
                        EventManager.Instance.Notice(new StartGameEvent());
                    }
                    else
                    {
                        MessageHelper.ShowMessage("Je suis désolé Dave, je ne crois pas pouvoir faire ça!", "Seul la princesse de place peut ouvrir le bal");
                    }
                    break;
                case UiEvent.EventInfo.OnlineBoard:
                    EnterSubstate(OnlineSubstate.OnlineBoard);
                    break;
                case UiEvent.EventInfo.HostGame:
                    EnterSubstate(OnlineSubstate.GameCreation);
                    break;
                case UiEvent.EventInfo.Back:
                    if (_currentSubState == OnlineSubstate.OnlineMenu)
                    {
                        EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.GameMenu });
                    }
                    else if (_currentSubState == OnlineSubstate.WaitingRoom)
                    {
                        if (_currentGame.IsHost)
                        {
                            EventManager.Instance.Interrupt(new TerminateSessionEvent()
                            {
                                Cause = TerminateSessionEvent.TerminationCause.PrematureQuit
                            });
                        }

                        ExitGame();
                        EnterSubstate(OnlineSubstate.OnlineBoard);
                    }
                    else
                    {
                        EnterSubstate(OnlineSubstate.OnlineMenu);
                    }

                    break;

                case UiEvent.EventInfo.MatchMaking:
                    var context = new MatchMakingViewModel();
                    var popup = new MatchMakingPopup(context);
                    popup.ShowDialog();
                    break;
            }
        }

        private void HandleOnlineSessionEvent(AbstractSessionEvent sessionEvent)
        {
            var terminateEvent = sessionEvent as TerminateSessionEvent;
            if (terminateEvent != null)
            {
                TerminateSessionEvent.ExplainCauseToUser(terminateEvent.Cause);
                if (IsGameRunning())
                {
                    EndGame(EndGameType.Disconnect);
                }
                ExitGame();
                EnterSubstate(OnlineSubstate.OnlineBoard);
            }

            var kickPlayerEvent = sessionEvent as KickPlayerEvent;
            if (kickPlayerEvent != null)
            {
                if (kickPlayerEvent.KickedPlayerId == Profile.Instance.CurrentProfile.UserHashId)
                {
                    MessageHelper.ShowMessage("Vous avez mal à votre popotin!", "Le puissant serviteur de la reine du bal vous à expulsé du bal à coup de pied sur les fesses!");
                    ExitGame();
                    EnterSubstate(OnlineSubstate.OnlineBoard);
                }
            }

            var playerReadyEvent = sessionEvent as PlayerReadyEvent;
            if (playerReadyEvent != null)
            {

            }

            var startOnlineGame = sessionEvent as StartGameEvent;
            if (startOnlineGame != null)
            {
                _currentGame.Start(false);
                _currentSubState = OnlineSubstate.InGame;
            }

            var endOnlineGame = sessionEvent as EndOnlineGameEvent;
            if (endOnlineGame != null)
            {
                Console.WriteLine("**************** Received EndOnlineGameEvent ****************");
                EndOnlineGameEvent.ExplainCauseToUser(endOnlineGame.EndCause);
                EndGame(endOnlineGame.EndCause);
                ExitGame();
                EnterSubstate(OnlineSubstate.OnlineBoard);
            }

            var playerForfeit = sessionEvent as PlayerForfeitEvent;
            if (playerForfeit != null)
            {
                Console.WriteLine("**************** A player had forfeit ****************");
            }

        }

        private void EndGame(EndGameType type)
        {
            _currentGame?.EndGame(type);
        }

        private void ExitGame()
        {
            if (_currentGame != null)
            {
                _currentGame.Exit();
                _currentGame = null;
            }
        }

        private void EnterSubstate(OnlineSubstate nextSubstate)
        {
            switch (nextSubstate)
            {
                case OnlineSubstate.OnlineMenu:
                    var onlineMenu = new OnlineMenu();
                    Program.MainWindow.SwitchScreen(onlineMenu);
                    break;
                case OnlineSubstate.OnlineBoard:
                    var board = new OnlineBoardPanel();
                    Program.MainWindow.SwitchScreen(board);
                    break;
                case OnlineSubstate.GameCreation:
                    var campaignConfig = new OnlineGameConfig();
                    campaignConfig.InitConfig();
                    campaignConfig.GotoConfigWindows();
                    break;
                case OnlineSubstate.WaitingRoom:
                    var waitingRoom = new WaitingRoom();
                    Program.MainWindow.SwitchScreen(waitingRoom);
                    break;
            }
            _currentSubState = nextSubstate;
        }

        public bool IsGameRunning()
        {
            return _currentGame != null && _currentGame.CurrentState == GameState.IsRunning;
        }
    }
}