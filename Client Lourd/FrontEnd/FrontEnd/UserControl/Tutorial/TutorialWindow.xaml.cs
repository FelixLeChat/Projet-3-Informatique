using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using FrontEnd.Player;
using FrontEndAccess.APIAccess;
using Helper.Image;
using Models;

namespace FrontEnd.UserControl.Tutorial
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TutorialWindow
    {
        private List<Bitmap> Images { get; set; }
        private int Step { get; set; } = 0;
        private EnumsModel.TutorialType Type { get; set; }

        public TutorialWindow(List<Bitmap> tutorialImages, EnumsModel.TutorialType type)
        {
            Type = type;
            InitializeComponent();
            Images = tutorialImages ?? new List<Bitmap>();
            InitTutorial();
        }

        private void InitTutorial()
        {
            CheckImage();
            CheckStep();
        }

        private void CheckStep()
        {
            if (Images.Count == 1 || Images.Count-1 == Step)
                DoneButton.Visibility = Visibility.Visible;
        }

        private void CheckImage()
        {
            BackgroundImage.Source = ImageHelper.LoadBitmap(Images[Step]);
        }

        private void DoneButton_MouseLeftButtonDown(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();

            if (Type == EnumsModel.TutorialType.Editor)
                Properties.Settings.Default.ShowTutorial = false;
            else
                Properties.Settings.Default.ShowGameTutorial = false;

            if (!User.Instance.IsConnected) return;
            if (Type == EnumsModel.TutorialType.Editor)
            {
                ProfileAccess.Instance.SetEditorTutorialVisibility(false);
                Profile.Instance.CurrentProfile.ShowEditorTutorial = false;
            }
            else
            {
                ProfileAccess.Instance.SetGameTutorialVisibility(false);
                Profile.Instance.CurrentProfile.ShowGameTutorial = false;
            }
        }

        private void Button_MouseLeftButtonDown(object sender, RoutedEventArgs routedEventArgs)
        {
            DoneButton_MouseLeftButtonDown(sender, routedEventArgs);
        }

        private void Button_MouseLeftButtonDown_1(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Step > 0)
            {
                Step--;
                CheckStep();
                CheckImage();
            }
        }

        private void Button_MouseLeftButtonDown_2(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Step < Images.Count - 1)
            {
                Step++;
                CheckStep();
                CheckImage();
            }
        }
    }
}
