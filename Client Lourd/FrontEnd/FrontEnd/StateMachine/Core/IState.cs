using FrontEnd.Core.Event;

namespace FrontEnd.StateMachine.Core
{
    /// <summary>
    /// Interface qui représente les états du jeu/menu possibles
    /// </summary>
    public interface IState
    {
            /// <summary>
            /// Appelé lorsqu'on entre dans cette état
            /// </summary>
            void EnterState();
            /// <summary>
            /// Appelé lorsqu'on quitte cette état
            /// </summary>
            void ExitState();
            /// <summary>
            /// Appelé à chaque frame
            /// </summary>
            /// <param name="deltaTime">Temps en miliseconde écoulé depuis le dernier frame</param>
            void Run(double deltaTime);
        /// <summary>
        /// Permet de s'abonner au événement du UI
        /// </summary>
        /// <param name="triggedEvent">Evenement</param>
        void Notice(IEvent triggeredEvent);
        }
}
