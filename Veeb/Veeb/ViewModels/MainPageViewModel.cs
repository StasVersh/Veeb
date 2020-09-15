using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Veeb.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {


        private double bpm = 60;
        private double FromSecender;
        private bool StapStart = true;

        private string _startBattonText = "Start";
        public string StartButtonText
        {
            get => _startBattonText;
            set => SetProperty(ref _startBattonText, value);
        }

        private string _entryText;
        public string EntryText
        {
            get => _entryText;
            set => SetProperty(ref _entryText, value);
        }

        private string _sliderText = "0";
        public string SliderText
        {
            get => _sliderText;
            set => SetProperty(ref _sliderText, value);
        }

        public DelegateCommand StartCommand { get; }
        public DelegateCommand SliderValue { get; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            StartCommand = new DelegateCommand(Start);
            SliderValue = new DelegateCommand(SliderBpm);
        }

        private void Start() 
        {
            FromSecender = 1 / (bpm / 60);
            Device.StartTimer(TimeSpan.FromSeconds(FromSecender), () =>
            {
                try
                {
                    Vibration.Cancel();
                    var duration = TimeSpan.FromSeconds(FromSecender/3);
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException ex)
                {
                }
                catch (Exception ex)
                {

                }
                return StapStart; 
            });
        }

        private void SliderBpm()
        {

        }
    }
}
