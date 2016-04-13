using System;
using System.Linq;
using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Helper;
using FrontEnd.ProfileHelper;
using FrontEnd.StateMachine.State;
using FrontEnd.ViewModel.Base;
using Models.Database;

namespace FrontEnd.Model.ViewModel
{
    public class MatchMakingViewModel : ObservableObject
    {
        public enum SearchState
        {
            Searching,
            Found,
            NotFound
        }
        static Random rnd = new Random();

        // Need to bind in the view
        public Action CloseAction { get; set; }

        // Game available to join
        public GameModel FoundGame { get; set; }

        private SearchState _currentState;
        public SearchState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                RaisePropertyChangedEvent(nameof(CurrentState));
            }
        }

        public MatchMakingViewModel()
        {
            CurrentState = SearchState.NotFound;

            // little hack sorry :(
            MatchMakingHostState.Popup = this;
            MatchMakingSearchingState.Popup = this;
        }

        public ICommand MatchMakingCommand => new DelegateCommand(EnterMatchMaking);

        private void EnterMatchMaking()
        {
            CurrentState = SearchState.Searching;
            EventManager.Instance.Notice(new ChangeStateEvent() { NextState = Enums.States.MatchMakingHosting });
        }

        public ICommand FindGameCommand => new DelegateCommand(FindGame);

        /// <summary>
        /// Try to find a game for the player
        /// </summary>
        public void FindGame()
        {
            CurrentState = SearchState.Searching;
            var games = JoinGameHelper.GetAvailableGameForPlayers();
            if (games.Any())
            {
                int r = rnd.Next(games.Count);
                FoundGame = games[r];
                CurrentState = SearchState.Found;
            }
            else
            {
                CurrentState = SearchState.NotFound;
                MessageHelper.ShowMessage("Personne ne cherche de belle princesse comme toi", "Aucune partie accessible n'a été trouvé, Vous pouvez chercher de nouveau ou héberger vous même la partie");
            }
        }

        public ICommand JoinGameCommand => new DelegateCommand(JoinGame);

        /// <summary>
        /// Join the found game
        /// </summary>
        public void JoinGame()
        {
            if (FoundGame == null)
            {
                Console.WriteLine("No game has been found, you can't join what doesn't exist");
                return;
            }

            EventManager.Instance.Interrupt(new ChangeStateEvent() { NextState = Enums.States.OnlineGame });
            var joinEvent = new JoinOnlineGameRequestEvent()
            {
                HashId = FoundGame.HashId,
                IsPrivate = FoundGame.IsPrivate,
                Name = FoundGame.Name
            };

            EventManager.Instance.Notice(joinEvent);
            CloseAction();
        }

        public ICommand HostGameCommand => new DelegateCommand(HostGame);

        /// <summary>
        /// Decide to not join any game and host one instead
        /// </summary>
        public void HostGame()
        {
            EventManager.Instance.Notice(new UiEvent() {Info = UiEvent.EventInfo.HostGame});
            CloseAction();
        }

        public ICommand QuitCommand => new DelegateCommand(Quit);

        /// <summary>
        /// Decide to not join any game and host one instead
        /// </summary>
        public void Quit()
        {
            EventManager.Instance.Notice(new ChangeStateEvent() {NextState = Enums.States.OnlineGame});
            CloseAction();
        }
    }
}