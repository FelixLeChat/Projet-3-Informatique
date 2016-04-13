using System;
using FrontEnd.Core;
using FrontEnd.Game.Config.Common;
using FrontEnd.Game.Points;
using FrontEnd.UserControl;
using FrontEnd.UserControl.Winform;
using FrontEndAccess.APIAccess;

namespace FrontEnd.Game.Wrap
{
    public class LocalGame : IGame
    {
        public IGameConfig Config { get; set; }
        private OpenGlPanel _windowOpengl;
        private readonly IntegratedOpenGl.Mode _mode;
        private readonly PointsManager _userPointsManager;

        public LocalGame(IntegratedOpenGl.Mode mode)
        {
            _mode = mode;
            
            _userPointsManager = new PointsManager(0);
        }

        public GameState CurrentState { get; private set; }

        public void Load()
        {
            CurrentState = GameState.Loading;
            Config.PreSetup();

            _windowOpengl = new OpenGlPanel();
            // Setup label for points
            _userPointsManager.PointJoueur1Label = _windowOpengl.PointJoueur1Label;
            _userPointsManager.BallLabel1 = _windowOpengl.BallJoueur1Label;
            _userPointsManager.PointJoueur1Label = _windowOpengl.PointJoueur1Label;


            Config.Setup();

            _windowOpengl.SelectMode(_mode);
            Program.MainWindow.SwitchScreen(_windowOpengl);
            Program.MainWindow.Hide();

            Program.resetTemps();
            NativeFunction.demarrerPartie();
            CurrentState = GameState.ReadyToStart;
        }

        public void Start()
        {
            CurrentState = GameState.IsRunning;
            _userPointsManager.StartGame(_mode);
        }

        public void Run(double deltaTime)
        {
            // Pointage 
            _userPointsManager.PointPlayer1 = NativeFunction.obtenirMonPointage();
            _userPointsManager.BallsPlayer1 = NativeFunction.obtenirMonNbBilles();

            _windowOpengl.Run(deltaTime);
        }

        public void EndGame(EndGameType type)
        {
            // Manage stats and achievements related to the game
            _userPointsManager.Endgame();
        }

        public void Exit()
        {
            NativeFunction.arreterSons();
            Program.MainWindow.Show();
        }

        public void EndGame()
        {
        }
    }
}