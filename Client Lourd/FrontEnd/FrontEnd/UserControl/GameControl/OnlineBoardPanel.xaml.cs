using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using FrontEnd.Core.Event;
using FrontEnd.ViewModel;
using FrontEndAccess.APIAccess;
using Models;
using EventManager = FrontEnd.Core.EventManager;
using System.Linq;
using System.Windows.Controls;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.ViewModel.Converter;

namespace FrontEnd.UserControl.GameControl
{
    /// <summary>
    /// UserControl where the player can see all availables game and can join them.
    /// </summary>
    public partial class OnlineBoardPanel
    {
        private ObservableCollection<GameViewModel> AvailableGames;

        public OnlineBoardPanel()
        {
            InitializeComponent();
            ReloadAvailableGame();

            // Change Title
            MainWindow.Instance.SwitchTitle("Liste des Parties");
        }

        public void ReloadAvailableGame()
        {
            var list = GameAccess.Instance.GetAllGames();
            var convertList = list.Select(GameModelConverter.ConvertGameModel).ToList();

            AvailableGames = new ObservableCollection<GameViewModel>(convertList);

            Table.ItemsSource = AvailableGames;
        }

        private void RefreshBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ReloadAvailableGame();
        }

        private void BackBtn_OnClick(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.Back });
        }

        private void JoinBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedGame = Table.SelectedItem as GameViewModel;
            if(selectedGame == null)
                throw new NullReferenceException("We should not be able to press join if no game is selected");
            EventManager.Instance.Notice(new JoinOnlineGameRequestEvent() { HashId = selectedGame.Id, IsPrivate = selectedGame.IsPrivate, Password = null, Name = selectedGame.Name});
        }

        private void HostBtn_OnClick(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.HostGame });
        }

        /// <summary>
        /// To prevent the HashId of the game to be shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.Name == "Id")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
        }

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JoinBtnGray.Visibility = Visibility.Hidden;
            JoinBtn.Visibility = Visibility.Hidden;
            SpectateBtn.Visibility = Visibility.Hidden;
            ReconnectBtn.Visibility = Visibility.Hidden;

            var item = Table.SelectedItem as GameViewModel;
            var choice = GetChoice(item);
            switch (choice)
            {
                case ButtonChoice.None:
                    JoinBtnGray.Visibility = Visibility.Visible;
                    break;
                case ButtonChoice.Join:
                    JoinBtn.Visibility = Visibility.Visible;
                    break;
                case ButtonChoice.Reconnect:
                    ReconnectBtn.Visibility = Visibility.Visible;
                    break;
                case ButtonChoice.Spectate:
                    SpectateBtn.Visibility = Visibility.Visible;
                    break;
            }
        }

        private enum ButtonChoice
        {
            None,
            Join,
            Spectate,
            Reconnect
        }

        private ButtonChoice GetChoice(GameViewModel selectedItem)
        {
            if (selectedItem == null)
            {
                return ButtonChoice.None;
            }
            else
            {
                try
                {
                    var gameModel = GameAccess.Instance.GetGameInfo(selectedItem.Id);

                    if (gameModel.DisconnectedHashId.Contains(Profile.Instance.CurrentProfile.UserHashId))
                    {
                        return ButtonChoice.Reconnect;
                    }
                    else if (selectedItem.State == EnumsModel.GameState.Started)
                    {
                        return ButtonChoice.Spectate;
                    }
                    else
                    {
                        return ButtonChoice.Join;
                    }
                }
                catch (Exception)
                {
                    MessageHelper.ShowMessage("Hum, il n'y a personne ici!",
                        "Ce bal est soit fini ou n'a jamais existé");
                    ReloadAvailableGame();
                }
                return ButtonChoice.None;
            }
        }

        private void SpectateBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedGame = Table.SelectedItem as GameViewModel;
            if (selectedGame == null)
                throw new NullReferenceException("We should not be able to press spectate if no game is selected");
            EventManager.Instance.Notice(new SpectateGameEvent() { HashId = selectedGame.Id });
        }
    }
}
