using System;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game.Config;
using FrontEnd.Game.Wrap;
using FrontEnd.StateMachine.Core;
using FrontEnd.UserControl;

namespace FrontEnd.StateMachine.State
{
    /// <summary>
    /// Will offer the config menu for offline game and manage the game flow (start, during, end)
    /// </summary>
    public class OfflineGameState : IState, IGameState
    {
        private IGame _currentGame;

        private enum OfflineSubstate
        {
            OfflineMenu,
            QuickConfig,
            CampaignConfig
        }

        private OfflineSubstate _currentSubState;

        public void EnterState()
        {
            EnterSubstate(OfflineSubstate.OfflineMenu);
        }

        public void ExitState()
        {
            if (_currentGame != null && _currentGame.CurrentState == GameState.IsRunning)
            {
                _currentGame.Exit();
            }
        }

        /// <summary>
        /// Need to update the current game if it is running
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Run(double deltaTime)
        {
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

            var startGame = triggeredEvent as CreateGameRequestEvent;
            if (startGame != null)
            {
                var config = startGame.Config;
                _currentGame = config.CreateGame();
                _currentGame.Load();
                _currentGame.Start();
            }

            var gameEnd = triggeredEvent as EndGameEvent;
            if (gameEnd != null)
            {
                EndGame();
                
            }
        }

        private void EndGame()
        {
            _currentGame.Exit();
            EnterSubstate(OfflineSubstate.OfflineMenu);
        }

        private void HandleUiEvent(UiEvent uiEvent)
        {
            switch (uiEvent.Info)
            {
                case UiEvent.EventInfo.QuickGame:
                    EnterSubstate(OfflineSubstate.QuickConfig);
                    break;
                case UiEvent.EventInfo.Campaign:
                    EnterSubstate(OfflineSubstate.CampaignConfig);
                    break;
                case UiEvent.EventInfo.Back:
                    if (_currentSubState == OfflineSubstate.OfflineMenu)
                    {
                        EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.GameMenu });
                    }
                    else
                    {
                        EnterSubstate(OfflineSubstate.OfflineMenu);
                    }
                    break;
            }
        }

        private void EnterSubstate(OfflineSubstate nextSubstate)
        {
            switch (nextSubstate)
            {
                case OfflineSubstate.OfflineMenu:
                    var offlineMenu = new OfflineMenu();
                    Program.MainWindow.SwitchScreen(offlineMenu);
                    break;
                case OfflineSubstate.QuickConfig:
                    var quickGameConfig = new QuickGameConfig();
                    quickGameConfig.InitConfig();
                    quickGameConfig.GotoConfigWindows();
                    break;
                case OfflineSubstate.CampaignConfig:
                    var campaignConfig = new CampaingGameConfig();
                    campaignConfig.InitConfig();
                    campaignConfig.GotoConfigWindows();
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