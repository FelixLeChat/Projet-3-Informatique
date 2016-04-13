using FrontEnd.Core.Event;

namespace FrontEnd.StateMachine.Core
{
    /// <summary>
    /// Interface qui représente les états du jeu/menu possibles
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// Return true if a game is running
        /// </summary>
        /// <returns></returns>
        bool IsGameRunning();
    }
}
