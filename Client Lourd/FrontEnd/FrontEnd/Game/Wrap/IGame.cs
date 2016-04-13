using FrontEnd.Annotations;
using FrontEnd.Core.Event;

namespace FrontEnd.Game.Wrap
{
    public enum GameState
    {
        Loading,
        ReadyToStart,
        IsRunning
    }

    public enum EndGameType
    {
        Dead,
        Forfeit,
        Disconnect,
        Won
    }

    /// <summary>
    /// Manage a running game (will open the winform, ...)
    /// </summary>
    public interface IGame
    {
        GameState CurrentState { get; }
        void Load();
        void Start();
        void Run(double deltaTime);
        void EndGame(EndGameType type);
        void Exit();
    }
}