using System;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;
using Helper.Image;

namespace FrontEnd.UserControl.AchievementControl
{
    /// <summary>
    /// Interaction logic for AchievementPanel.xaml
    /// </summary>
    public partial class AchievementPanel 
    {
        private static System.Timers.Timer _timer;

        public AchievementPanel()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer {Interval = 4000};
            _timer.Elapsed += (sender, args) =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (HideAchievement));
            };
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            HideAchievement();
        }

        public void ShowAchievement(Bitmap image, bool isAchievement = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
            {
                _timer.Stop();
                _timer.Start();

                MusicLabel.Visibility = Visibility.Collapsed;
                AchievementLabel.Visibility = Visibility.Visible;

                Visibility = Visibility.Visible;
                if (image != null)
                    AchievementImage.Fill = ImageHelper.GetImageBrush(image);

                if (!isAchievement)
                {
                    MusicLabel.Visibility = Visibility.Visible;
                    AchievementLabel.Visibility = Visibility.Collapsed;
                }
            }));
        }

        public void HideAchievement()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                Visibility = Visibility.Collapsed;
                _timer.Stop();
            }));
        }
    }
}
