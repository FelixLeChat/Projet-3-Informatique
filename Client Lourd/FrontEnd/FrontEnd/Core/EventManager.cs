using System;
using System.Windows;
using FrontEnd.Core.Event;

namespace FrontEnd.Core
{
    /// <summary>
    /// Manager permettant de gérer les événements (recoit et dispatch les événements)
    /// Singleton via Instance
    /// </summary>
    public class EventManager
    {
        private static EventManager _mInstance;

        public static EventManager Instance
        {
            get
            {
                if (_mInstance == null)
                {
                    _mInstance = new EventManager();
                }
                return _mInstance;
            }
        }

        // Signature de l'évenement
        public delegate void EventHandlerDelegate(IEvent triggeredEvent);

        private event EventHandlerDelegate EventTreshold;

        private EventManager()
        {
        }

        /// <summary>
        /// Permet de subscribe aux événements
        /// </summary>
        public void Subscribe(EventHandlerDelegate eh)
        {
            EventTreshold += eh;
        }

        /// <summary>
        /// Permet de unsubscribe aux événements
        /// </summary>
        public void Unsubscribe(EventHandlerDelegate eh)
        {
            EventTreshold -= eh;
        }

        /// <summary>
        /// Pour dispatch un événements
        /// </summary>
        public void Notice(IEvent triggeredEvent)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => EventTreshold?.Invoke(triggeredEvent)));
        }

        /// <summary>
        /// Pour dispatch un événements immediatement (hack)
        /// </summary>
        public void Interrupt(IEvent triggeredEvent)
        {
            Application.Current.Dispatcher.Invoke(() => EventTreshold?.Invoke(triggeredEvent));
        }
    }
}
