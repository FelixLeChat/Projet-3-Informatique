using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FrontEnd.Achievements;
using FrontEnd.AsyncLoading;
using FrontEnd.Core;
using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Game.Config.Helper;
using FrontEnd.Game.Points;
using FrontEnd.Helper;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.UserControl;
using FrontEnd.UserControl.Winform;
using FrontEndAccess.APIAccess;
using Models.Database;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.Game.Wrap
{
    /// <summary>
    /// Represent an online game (to use with OnlineSession)
    /// Handle game start (initiate C++,...), end and exit
    /// </summary>
    public class OnlineGame : IGame
    {
        public bool IsHost { get; set; }
        public GameModel Model { get; private set; }
        private OpenGlPanel _windowOpengl;
        private readonly IntegratedOpenGl.Mode _mode;
        private readonly PointsManager _userPointsManager;

        private List<Zone> _zones;

        CancellationTokenSource cts;

        public GameState CurrentState { get; private set; }


        public OnlineGame(IntegratedOpenGl.Mode mode, GameModel model)
        {
            _mode = mode;
            Model = model;
            OnlineSession.InitializeSession(model);

            var playerPos = Model.ParticipantsHashId.IndexOf(Profile.Instance.CurrentProfile.UserHashId);
            _userPointsManager = new PointsManager(playerPos);

            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Task.Factory.StartNew(() => UpdateGameModelTask(token), token);
        }



        public void Load()
        {
            CurrentState = GameState.Loading;

            // Download zone
            _zones = ZoneSynchronizer.DownloadZoneFromHashId(Model.ZonesHashId);

            CurrentState = GameState.ReadyToStart;

            //Todo: Send Player ready event
            EventManager.Instance.Notice(new PlayerReadyEvent()
            {
                PlayerId = Profile.Instance.CurrentProfile.UserHashId
            });
        }

        public void Start()
        {
            // To keep last behavior
            Start(false);
        }

        /// <summary>
        /// Init the c++ side
        /// </summary>
        private void InitBackend(bool isReconnect)
        {
            // Todo: Recheck after zone download is complete
            var helper = new OnlineConfigHelper(Model, _zones);

            // To check if it is a quickgame or campaing
            if (Model.ZonesHashId.Count > 1)
            {
                NativeFunction.creerPartieCampagneEnLigne(helper.IsCoop, helper.PlayerCount, helper.IaCount,
                    helper.MatchId, helper.MathIdLength, helper.PlayersId, helper.PlayersIdLength, helper.ZoneCount, helper.ZonesPath,
                    helper.ZonesPathLength, IsHost, isReconnect);

            }
            else
            {
                NativeFunction.creerPartieSimpleEnLigne(helper.IsCoop, helper.PlayerCount, helper.IaCount,
                    helper.MatchId, helper.MathIdLength, helper.PlayersId, helper.PlayersIdLength, new StringBuilder(helper.ZonesPath[0]),
                    helper.ZonesPathLength[0], IsHost, isReconnect);
            }
        }

        public void Start(bool isReconnect)
        {
            GameAchievementHelper.CheckForAchievementTask(_zones);

            _windowOpengl = new OpenGlPanel();
            _windowOpengl.SelectMode(_mode);

            // Setup label for points
            _userPointsManager.PointJoueur1Label = _windowOpengl.PointJoueur1Label;
            _userPointsManager.PointJoueur2Label = _windowOpengl.PointJoueur2Label;
            _userPointsManager.PointJoueur3Label = _windowOpengl.PointJoueur3Label;
            _userPointsManager.PointJoueur4Label = _windowOpengl.PointJoueur4Label;

            _userPointsManager.BallLabel1 = _windowOpengl.BallJoueur1Label;
            _userPointsManager.BallLabel2 = _windowOpengl.BallJoueur2Label;
            _userPointsManager.BallLabel3 = _windowOpengl.BallJoueur3Label;
            _userPointsManager.BallLabel4 = _windowOpengl.BallJoueur4Label;

            InitBackend(isReconnect);

            Program.MainWindow.SwitchScreen(_windowOpengl);
            Program.MainWindow.Hide();

            Program.resetTemps();
            NativeFunction.demarrerPartie();

            CurrentState = GameState.IsRunning;

            _userPointsManager.StartGame(_mode, true, isCompe: !Model.IsCoop, numbJoueurs: Model.MaxPlayersCount);
        }

        public void Run(double deltaTime)
        {
            // Get point if mode if competitive
            if (!Model.IsCoop)
            {
                var playerCount = Model.MaxPlayersCount;

                _userPointsManager.PointPlayer1 = NativeFunction.obtenirPointageJoueur(0);
                if (playerCount > 1)
                    _userPointsManager.PointPlayer2 = NativeFunction.obtenirPointageJoueur(1);
                if (playerCount > 2)
                    _userPointsManager.PointPlayer3 = NativeFunction.obtenirPointageJoueur(2);
                if (playerCount > 3)
                    _userPointsManager.PointPlayer4 = NativeFunction.obtenirPointageJoueur(3);

                _userPointsManager.BallsPlayer1 = NativeFunction.obtenirNbBillesJoueur(0);
                if (playerCount > 1)
                    _userPointsManager.BallsPlayer2 = NativeFunction.obtenirNbBillesJoueur(1);
                if (playerCount > 2)
                    _userPointsManager.BallsPlayer3 = NativeFunction.obtenirNbBillesJoueur(2);
                if (playerCount > 3)
                    _userPointsManager.BallsPlayer4 = NativeFunction.obtenirNbBillesJoueur(3);
            }
            else
            {
                // Pointage 
                _userPointsManager.PointPlayer1 = NativeFunction.obtenirMonPointage();
                _userPointsManager.BallsPlayer1 = NativeFunction.obtenirMonNbBilles();
            }

            _windowOpengl.Run(deltaTime);
        }


        private const int ErrorCountLimit = 3;

        private void UpdateGameModelTask(CancellationToken ct)
        {
            int updateErrorCount = 0;
            while (!cts.IsCancellationRequested)
            {
                // If the online session was terminated or had changed
                if (OnlineSession.Instance == null || OnlineSession.Instance.SessionId != Model.HashId)
                {
                    break;
                }

                if (TryUpdateGameModel())
                {
                    updateErrorCount = 0;
                }
                else
                {
                    updateErrorCount++;
                    if (updateErrorCount >= ErrorCountLimit)
                    {

                        var terminateSessionEvent = new TerminateSessionEvent()
                        {
                            SendToServer = false,
                            Cause = TerminateSessionEvent.TerminationCause.HostDisconnection
                        };
                        EventManager.Instance.Notice(terminateSessionEvent);
                    }
                }
                Thread.Sleep(500);
            }
        }

        private bool TryUpdateGameModel()
        {
            if (!ConnectionHelper.IsConnected)
            {
                return false;
            }

            bool connectionExist = true;
            try
            {
                var updatedModel = GameAccess.Instance.GetGameInfo(Model.HashId);
                Model = updatedModel;
                Application.Current.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        OnlineSession.Instance?.UpdateParticipantsList(Model);
                    }));

            }
            catch (Exception e)
            {
                // TODO: Handle disconnection!
                Console.WriteLine("Error while updating the model: {0}", e.Message);


                connectionExist = false;
            }
            return connectionExist;
        }

        public bool IsQuickGame()
        {
            return Model.ZonesHashId.Count == 1;
        }

        public bool IsSpectator()
        {
            return Model.SpectatorsHashId.Contains(Profile.Instance.CurrentProfile.UserHashId);
        }

        /// <summary>
        /// Method called when the game is ended (not yet exited)
        /// </summary>
        /// <param name="type"></param>
        public void EndGame(EndGameType type)
        {
            switch (type)
            {
                case EndGameType.Dead:
                    if (IsSpectator())
                    {
                        MessageHelper.ShowMessage("Vous êtes belle!", "En ne faisant que regarder, vous avez vue votre prince-charmant partir dans le carosse de la belle rouqine avec des éphélides");
                    }
                    else
                    {
                        // The game can't be won
                        if (IsQuickGame() && Model.IsCoop)
                        {
                            MessageHelper.ShowMessage("Belle performance", $"Vous avez bien dansé, votre prince charmant vous donne un pointage de {_userPointsManager.GetMyPoints()}!");
                        }
                        else
                        {
                            MessageHelper.ShowMessage("Défaite", "Oh non! Votre affreuse belle-soeur a séduit votre prince charmant!!!");
                        }
                    }
                    break;
                case EndGameType.Won:
                    MessageHelper.ShowMessage("Victoire", "Magnifique! Gagner vous a donné de la victoire!!!");
                    break;
                case EndGameType.Disconnect:
                    break;

                case EndGameType.Forfeit:
                    EventManager.Instance.Interrupt(new PlayerForfeitEvent(Profile.Instance.CurrentProfile.UserHashId));
                    if (IsHost)
                    {
                        EventManager.Instance.Interrupt(new EndOnlineGameEvent()
                        {
                            EndCause = EndGameType.Forfeit
                        });
                    }
                    break;
            }
            EndGameStat(type);
        }

        private void EndGameStat(EndGameType type)
        {
            if (ConnectionHelper.IsConnected)
            {
                try
                {
                    _userPointsManager.Endgame(type == EndGameType.Won);

                    // Can only add score for quick game (because we know the total point)
                    if (IsQuickGame())
                    {
                        LeaderboardAccess.Instance.AddLeaderEntry(Model.ZonesHashId.FirstOrDefault(), new LeaderModel()
                        {
                            PlayerHashId = Profile.Instance.CurrentProfile.UserHashId,
                            PlayerName = Profile.Instance.CurrentProfile.Username,
                            Points = _userPointsManager.GetMyPoints()
                        });
                    }

                }
                catch (Exception)
                {
                    // ignored
                }

            }
        }

        /// <summary>
        /// Called after EndGame
        /// </summary>
        public void Exit()
        {
            NativeFunction.arreterSons();
            cts.Cancel();
            OnlineSession.TerminateSession();

            _windowOpengl?.HideWinform();

            Program.MainWindow.Show();
        }


    }
}