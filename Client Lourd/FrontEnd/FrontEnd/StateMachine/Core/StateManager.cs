using System.Collections.Generic;
using System.Linq;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.ProfileHelper;
using FrontEnd.Setting;
using FrontEnd.StateMachine.State;
using FrontEnd.UserControl;

namespace FrontEnd.StateMachine.Core
{
    /// <summary>
    /// Classe qui permet de gérer les états et leurs transitions 
    /// </summary>
    public class StateManager
    {
        private IState _currentState;

        private static StateManager _instance;
        public static StateManager Instance
        {
            get { return _instance ?? (_instance = new StateManager()); }
        }

        private readonly Dictionary<int, IState> _stateDictionnary;

        /// <summary>
        /// Link state and Enums
        /// </summary>
        private StateManager()
        {
            _stateDictionnary = new Dictionary<int, IState>();

            IState state = new MainMenuState();
            _stateDictionnary.Add((int)Enums.States.MainMenu, state);

            state = new GameMenuState();
            _stateDictionnary.Add((int)Enums.States.GameMenu, state);

            state = new OfflineGameState();
            _stateDictionnary.Add((int)Enums.States.OfflineGame, state);

            state = new OnlineGameState();
            _stateDictionnary.Add((int)Enums.States.OnlineGame, state);

            state = new OptionMenuState();
            _stateDictionnary.Add((int)Enums.States.Options, state);

            state = new LoginMenuState();
            _stateDictionnary.Add((int)Enums.States.Login, state);

            state = new RegisterMenuState();
            _stateDictionnary.Add((int)Enums.States.Register, state);

            state = new ProfileMenuState();
            _stateDictionnary.Add((int)Enums.States.Profile, state);

            state = new EditionState();
            _stateDictionnary.Add((int)Enums.States.Edition, state);

            state = new ProfileEditMenuState();
            _stateDictionnary.Add((int)Enums.States.ProfileEdit, state);

            state = new UserFriendsState();
            _stateDictionnary.Add((int) Enums.States.UserFriends, state);

            state = new UserZoneMenuState();
            _stateDictionnary.Add((int) Enums.States.UserZones, state);

            state = new MatchMakingHostState();
            _stateDictionnary.Add((int)Enums.States.MatchMakingHosting, state);

            state = new MatchMakingSearchingState();
            _stateDictionnary.Add((int)Enums.States.MatchMakingSearching, state);

            EventManager.Instance.Subscribe(Notice);
        }

        /// <summary>
        /// Enter in the starting state
        /// </summary>
        public void Start()
        {
            _currentState = _stateDictionnary[(int)Enums.States.MainMenu];
            _currentState.EnterState();
        }

        /// <summary>
        /// Run current state
        /// </summary>
        public void Run(double deltaTime)
        {
            _currentState.Run(deltaTime);
            //Program.s_gameWindow.Run(deltaTime);
        }

        /// <summary>
        /// Get noticed when a change of state need to be done
        /// </summary>
        /// <param name="eventInfo"></param>
        /// <param name="info"></param>

        public void Notice(IEvent triggeredEvent)
        {
            
            var stateChangeEvent = triggeredEvent as ChangeStateEvent;
            if (stateChangeEvent != null)
            {
                ChangeState(stateChangeEvent.NextState);
            }
            else
            {
                _currentState.Notice(triggeredEvent);
            }
        }

        /// <summary>
        /// Permet d'obtenir l'état courant
        /// </summary>
        public Enums.States GetCurrentState()
        {
            return (Enums.States)_stateDictionnary.FirstOrDefault(x => x.Value == _currentState).Key;
        }

        public bool IsGameRunning()
        {
            var gameState = _currentState as IGameState;
            if (gameState != null)
            {
                return gameState.IsGameRunning();
            }

            var editionState = _currentState as EditionState;
            if (editionState != null)
            {
                return editionState.IsInTestMode();
            }
            return false;
        }

        /// <summary>
        /// Change state to next state by exiting current state and entering next one
        /// </summary>
        public void ChangeState(Enums.States nextState)
        {
            if (Options.Instance.IsLoading &&
                (nextState == Enums.States.Edition || 
                 nextState == Enums.States.OfflineGame ||
                 nextState == Enums.States.OnlineGame))
            {
                MessageHelper.ShowMessage("Musique en chargement", "La musique est en train d'etre charger, veuillez patienter un peu");
                return;
            }
            _currentState.ExitState();
            _currentState = _stateDictionnary[(int)nextState];
            _currentState.EnterState();
        }
    }
}
