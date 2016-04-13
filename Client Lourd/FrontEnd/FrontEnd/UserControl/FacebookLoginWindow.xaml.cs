using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for FacebookLoginWindow.xaml
    /// </summary>
    public partial class FacebookLoginWindow
    {
        //The Application ID from Facebook
        public string AppId { get; set; }

        //The access token retrieved from facebook's authentication
        public string AccessToken { get; set; }

        public FacebookLoginWindow()
        {
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                //Add the message hook in the code behind since I got a weird bug when trying to do it in the XAML
                WebBrowser.MessageHook += WebBrowser_MessageHook;

                //Delete the cookies since the last authentication
                DeleteFacebookCookie();

                //Create the destination URL
                var destinationUrl =
                    $"https://www.facebook.com/dialog/oauth?client_id={AppId}&scope={"email,user_birthday,publish_actions"}&display=popup&redirect_uri=http://www.facebook.com/connect/login_success.html&response_type=token";
                WebBrowser.Navigate(destinationUrl);
            };

            HideScriptErrors(WebBrowser, true);
        }

        private void WebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            //If the URL has an access_token, grab it and walk away...
            var url = e.Uri.Fragment;
            if (!url.Contains("access_token") || !url.Contains("#")) return;
            url = (new Regex("#")).Replace(url, "?", 1);
            AccessToken = HttpUtility.ParseQueryString(url).Get("access_token");
            DialogResult = true;
            Close();
        }

        private static void DeleteFacebookCookie()
        {
            //Set the current user cookie to have expired yesterday
            string cookie =
                $"c_user=; expires={DateTime.UtcNow.AddDays(-1).ToString("R"):R}; path=/; domain=.facebook.com";
            Application.SetCookie(new Uri("https://www.facebook.com"), cookie);
        }

        private void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri.LocalPath != "/r.php") return;
            MessageBox.Show("To create a new account go to www.facebook.com", "Could Not Create Account", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Cancel = true;
        }

        private IntPtr WebBrowser_MessageHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //msg = 130 is the last call for when the window gets closed on a window.close() in javascript
            if (msg == 130)
            {
                Close();
            }
            return IntPtr.Zero;
        }

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        private void FacebookLoginWindow_OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            DialogResult = true;
        }
    }
}
