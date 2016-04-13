using FrontEnd.ViewModel.Base;
using Models;

namespace FrontEnd.Game
{
    /// <summary>
    /// Represent a participant in a online game session
    /// </summary>
    public class SessionParticipant : ObservableObject
    {
        public enum ParticipantState
        {
            Setuping = 0,
            ReadyToStart,
            InGame
        }

        public Model.Player Player { get; set; }

        public string HashId { get { return Player.HashId; } }

        private ParticipantState _currentState;
        public ParticipantState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                RaisePropertyChangedEvent(nameof(CurrentState));
            }
        }
    }
}