using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrontEnd.Model.ViewModel;
using FrontEnd.ProfileHelper;

namespace FrontEnd.Game.Config
{
    /// <summary>
    /// Use as DataContext in a view for selecting a single map 
    /// </summary>
    public class SingleMapConfig
    {
        public SingleMapConfig(List<ZoneViewModel> possiblesZones)
        {
            PossiblesZones = new ObservableCollection<ZoneViewModel>(possiblesZones);
        }

        public ObservableCollection<ZoneViewModel> PossiblesZones { get; set; }
        public ZoneViewModel SelectedZone { get; set; }

        public bool Validate()
        {
            if (SelectedZone == null)
            {
                MessageHelper.ShowMessage("Woah minute!!!", "Un petit bal, demande au moins une danse!");
                return false;
            }
            return true;
        }
    }
}
