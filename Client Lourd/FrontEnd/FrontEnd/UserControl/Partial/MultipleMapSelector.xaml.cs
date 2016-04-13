using System.Windows;
using System.Windows.Controls;
using FrontEnd.Game;
using FrontEnd.Game.Config;
using FrontEnd.Model.ViewModel;
using FrontEnd.ModelConverter;

namespace FrontEnd.UserControl.Partial
{
    /// <summary>
    /// Interaction logic for MultipleMapSelector.xaml
    /// </summary>
    public partial class MultipleMapSelector
    {
        private MultiMapConfig _mapConfig;

        public MultipleMapSelector()
        {
            InitializeComponent();
        }

        public void SetDataContext(MultiMapConfig config)
        {
            _mapConfig = config;
            PossibleZonesListBox.ItemsSource = config.PossiblesZones;
            SelectedZonesListBox.ItemsSource = config.SelectedZones;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = PossibleZonesListBox.SelectedItem as ZoneViewModel;
            if (selectedItem != null)
            {
                _mapConfig.SelectedZones.Add(selectedItem);
            }

            // Will bubble up the events to the parents
            var newEventArgs = new RoutedEventArgs(MyCustomEvent);
            RaiseEvent(newEventArgs);
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = SelectedZonesListBox.SelectedItem as ZoneViewModel;
            if (selectedItem != null)
            {
                _mapConfig.SelectedZones.Remove(selectedItem);
            }

            // Will bubble up the events to the parents
            var newEventArgs = new RoutedEventArgs(MyCustomEvent);
            RaiseEvent(newEventArgs);
        }

    }
}
