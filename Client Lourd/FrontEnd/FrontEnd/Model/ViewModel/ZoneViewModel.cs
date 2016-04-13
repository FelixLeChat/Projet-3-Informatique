using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Game;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.ViewModel.Base;
using FrontEndAccess.APIAccess;
using Models;

namespace FrontEnd.Model.ViewModel
{
    /// <summary>
    /// Represent a participant in a online game session
    /// </summary>
    public class ZoneViewModel : ObservableObject
    {
        public string HashId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }

        private string _imagePath { get; set; }
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                RaisePropertyChangedEvent(nameof(ImagePath));
            }
        }

        private bool _isLoading = true;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChangedEvent(nameof(IsLoading));
            }
        }

        public static void LoadImagesAsync(List<ZoneViewModel> zones)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var zone in zones)
                {
                    zone.ImagePath = ZoneHelper.SaveImage(zone.HashId, zone.Name, true);
                    zone.IsLoading = false;
                }
            });
        }
    }
}