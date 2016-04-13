using System;
using System.Windows.Input;
using FrontEnd.UserControl;

namespace FrontEnd
{
    public partial class ChatWindow
    {
        private readonly ChatMenu _currentChatMenu;
        public ChatWindow(ChatMenu chatMenu)
        {
            InitializeComponent();
            ChatPanel.Content = chatMenu;
            _currentChatMenu = chatMenu;
        }

        private void ChatWindow_OnClosed(object sender, EventArgs e)
        {
            _currentChatMenu.OnWindowClosing();
        }

        private void Chat_MouseDown(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
