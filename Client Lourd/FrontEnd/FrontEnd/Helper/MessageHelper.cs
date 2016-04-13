using System;
using System.Web.Http.Common;
using System.Windows;
using FrontEnd.ViewModel;

namespace FrontEnd.ProfileHelper
{
    /// <summary>
    /// To show the user some message
    /// </summary>
    public static class MessageHelper
    {
        public static void ShowMessage(string title, string message)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new Action(() => MainWindow.Instance.SetMessage(new MessagePresenter(title, message))));
        }

        public static void ShowError(string title, string message, Exception e)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new Action(() => MainWindow.Instance.SetMessage(new MessagePresenter(title, message))));
            Console.WriteLine("--------------------  Exception  -----------------");
            Console.WriteLine("Title: {0}", title);
            Console.WriteLine("Message show to user: {0}", message);
            Console.WriteLine("Exception message: {0}", e.Message);
            Console.WriteLine("----- Stack -----");
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("----------------- Exception End  --------------");
        }

    }
}