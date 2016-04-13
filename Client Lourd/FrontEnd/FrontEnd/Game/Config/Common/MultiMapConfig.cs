using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrontEnd.Model.ViewModel;
using FrontEnd.ProfileHelper;


namespace FrontEnd.Game.Config
{
    /// <summary>
    /// Use as DataContext in a view for selecting a single map 
    /// </summary>
    public class MultiMapConfig
    {
        const int ZoneMimimunCount = 2;

        public MultiMapConfig(List<ZoneViewModel> possiblesZones, List<ZoneViewModel> selectedZones)
        {
            PossiblesZones = new ObservableCollection<ZoneViewModel>(possiblesZones);
            SelectedZones = new ObservableCollection<ZoneViewModel>(selectedZones);
        }

        public ObservableCollection<ZoneViewModel> PossiblesZones { get; set; }
        public ObservableCollection<ZoneViewModel> SelectedZones { get; set; }

        public bool Validate()
        {
            if (SelectedZones.Count < ZoneMimimunCount)
            {
                MessageHelper.ShowMessage("Woah minute!!!", $"Un bal de telle envergure demande au moins {ZoneMimimunCount} bals!");
                return false;
            }
            return true;
        }
    }
}
