using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace Veeb.ViewModels
{
    public class SettingsTabPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        public DelegateCommand toMetronomeSettingsCommand { get; set; }

        public SettingsTabPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            toMetronomeSettingsCommand = new DelegateCommand(toMetronomeSettings); ;
        }

        private async void toMetronomeSettings()
        {
            await _navigationService.NavigateAsync("MetronomeSettings");
        }
    } 
} 
