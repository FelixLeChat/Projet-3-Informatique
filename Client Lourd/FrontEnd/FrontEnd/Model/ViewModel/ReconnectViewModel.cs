using System;
using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.ViewModel.Base;
using Models.Database;

namespace FrontEnd.Model.ViewModel
{
    /// <summary>
    /// View model for the popup to ask about to reconnect from a disconnected game (on login)
    /// </summary>
    public class ReconnectViewModel : ObservableObject
    {
        // Need to bind in the view
        public Action CloseAction { get; set; }

        private GameModel _model;

        private string _gameName;
        public string GameName
        {
            get { return _gameName; }
            set
            {
                _gameName = value;
                RaisePropertyChangedEvent(nameof(GameName));
            }
        }


        public ReconnectViewModel(GameModel model)
        {
            _model = model;
            GameName = model.Name;
        }

        public ICommand JoinCommand => new DelegateCommand(Join);

        private void Join()
        {
           EventManager.Instance.Interrupt(new ChangeStateEvent() {NextState = Enums.States.OnlineGame});
            EventManager.Instance.Notice(new ReconnectToGameEvent() {Game = _model });
            CloseAction();
        }

        public ICommand ForfeitCommand => new DelegateCommand(Forfeit);

        private void Forfeit()
        {
            Console.WriteLine("Forfeit");
            CloseAction();
        }
    }
}