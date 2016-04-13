using System;
using System.Windows;

namespace FrontEnd.UserControl
{
    public partial class ChatCanalInputWindow
    {
        public ChatCanalInputWindow(string question, string defaultAnswer = "")
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            var name = txtAnswer.Text;
            if (name.Length > 8 || name.Length <= 0)
            {
                Error.Content = "Le nom doit être entre 1 et 8 caractères";
                return;
            }

            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }
    }
}