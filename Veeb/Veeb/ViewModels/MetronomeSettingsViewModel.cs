using Prism.Navigation;
using Veeb.Models;

namespace Veeb.ViewModels
{
    public class MetronomeSettingsViewModel : ViewModelBase
    {
        private int soud_volume;
        public int SoundVolume
        {
            get => soud_volume;
            set
            {
                SetProperty(ref soud_volume, value);
            }
        }

        private int vibration_force;
        public int VibrationForce
        {
            get => vibration_force;
            set
            {
                SetProperty(ref vibration_force, value);
            }
        }

        private string up_boundaries;
        public string UpBoundaries
        {
            get => up_boundaries;
            set
            {
                SetProperty(ref up_boundaries, value); ;
            }
        }

        private string down_boundaries;
        public string DownBoundaries
        {
            get => down_boundaries;
            set
            {
                SetProperty(ref down_boundaries, value);
            }
        }
        public MetronomeSettingsViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }
    }
}
