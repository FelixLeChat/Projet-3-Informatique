using System.Windows;
using System.Windows.Controls;
using FrontEnd.Game;
using FrontEnd.Game.Config;
using FrontEnd.Model.ViewModel;
using FrontEnd.ModelConverter;

namespace FrontEnd.UserControl.Partial
{
    /// <summary>
    /// Interaction logic for SingleMapSelector.xaml
    /// </summary>
    public partial class SingleMapSelector
    {
        private SingleMapConfig _mapConfig;

        public SingleMapSelector()
        {
            InitializeComponent();
        }

        public void SetDataContext(SingleMapConfig config)
        {
            _mapConfig = config;
            PossibleZonesListBox.ItemsSource = config.PossiblesZones;
        }

        private void PossibleZonesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((ListBox) sender).SelectedItem as ZoneViewModel;
            if (selectedItem != null)
            {
                _mapConfig.SelectedZone = selectedItem;
            }

            // Will bubble up the events to the parents
            var newEventArgs = new RoutedEventArgs(MyCustomEvent);
            RaiseEvent(newEventArgs);
        }

    }
}
