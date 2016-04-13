using System.Windows.Input;
using FrontEnd.ViewModel.Base;

namespace FrontEnd.ViewModel
{
    public class MessagePresenter : ObservableObject
    {
        public MessagePresenter(string title = "", string message = "")
        {
            Title = title;
            Message = message;
        }
        
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChangedEvent(nameof(IsVisible));
            }
        }

        private string _title;

        public string Title {
            get {return _title; }
            set
            {
                _title = value;
                RaisePropertyChangedEvent(nameof(Title));
            }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChangedEvent(nameof(Message));
            }
        }

        public ICommand CloseCommand
        {
            get { return new DelegateCommand(Close); }
        }

        private void Close()
        {
            IsVisible = false;
        }
    }
}