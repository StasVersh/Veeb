using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;
using Veeb.Models;
using Veeb.Resources;
using Xamarin.Forms;

namespace Veeb.ViewModels
{
    public class MetronomTabPageViewModel : BindableBase
    {
        #region Vars
        private double bpm = 120;
        private double FromSecender;
        private bool StartOn = false;
        private bool SoundOn = false;
        private bool VibroOn = false;
        private bool UpdateOn = false;
        private Metronome metronome;
        #endregion

        #region Strings
        public string BpmTextEntry
        {
            get => bpm.ToString("###");
            set
            {
                if (double.TryParse(value, out double bpm))
                {
                    Bpm = bpm;
                }
                RaisePropertyChanged(nameof(BpmTextEntry));
            }
        }
        public double Bpm
        {
            get => bpm;
            set
            {
                SetProperty(ref bpm, value);
                RaisePropertyChanged(nameof(BpmTextEntry));
                if (bpm <= 52) TempoText = "Largo";
                if (bpm > 52 & bpm <= 56) TempoText = "Adagio"; 
                if (bpm > 56 & bpm <= 72) TempoText = "Andante"; 
                if (bpm > 72 & bpm <= 92) TempoText = "Andantino"; 
                if (bpm > 92 & bpm <= 108) TempoText = "Allegretto"; 
                if (bpm > 108 & bpm <= 144) TempoText = "Allegro"; 
                if (bpm > 144 & bpm <= 176) TempoText = "Vivo"; 
                if (bpm > 176) TempoText = "Presto"; 
            }
        }

        private string tempo_text;
        public string TempoText
        {
            get => tempo_text;
            set
            {
                SetProperty(ref tempo_text, value);
            }
        }

        private double bpm_slider;
        public double BpmSlider
        {
            get
            {
                if(BpmSlider > 10)
                {
                    Bpm = BpmSlider;
                    bpm_slider = BpmSlider;
                }
                return BpmSlider;
            }
            set
            {
                SetProperty(ref bpm_slider, value);
            }
        }

        private string _textStartAndStopMetronomeButton = "Start";
        public string TextStartAndStopMetronomeButton
        {
            get => _textStartAndStopMetronomeButton;
            set => SetProperty(ref _textStartAndStopMetronomeButton, value);
        }
        private string _textOnAndOffSoundButton = "Off";
        public string TextOnAndOffSoundButton
        {
            get => _textOnAndOffSoundButton;
            set => SetProperty(ref _textOnAndOffSoundButton, value);
        }
        private string _textOnAndOffVibrationButton = "Off";
        public string TextOnAndOffVibrationButton
        {
            get => _textOnAndOffVibrationButton;
            set => SetProperty(ref _textOnAndOffVibrationButton, value);
        }
        private string _tapingTempoButtonText = "Taping Tempo";
        public string TapingTempoButtonText
        {
            get => _tapingTempoButtonText;
            set => SetProperty(ref _tapingTempoButtonText, value);
        }
        #endregion

        #region Colors
        private Color _backgroundColorStartAndStopMetronomeButton = Colors.Passive;
        public Color BackgroundColorStartAndStopMetronomeButton
        {
            get => _backgroundColorStartAndStopMetronomeButton;
            set => SetProperty(ref _backgroundColorStartAndStopMetronomeButton, value);
        }
        private Color _backgroundColorOnAndOffSoundButton = Colors.Passive;
        public Color BackgroundColorOnAndOffSoundButton
        {
            get => _backgroundColorOnAndOffSoundButton;
            set => SetProperty(ref _backgroundColorOnAndOffSoundButton, value);
        }
        private Color _backgroundColorOnAndOffVibrationButton = Colors.Passive;
        public Color BackgroundColorOnAndOffVibrationButton
        {
            get => _backgroundColorOnAndOffVibrationButton;
            set => SetProperty(ref _backgroundColorOnAndOffVibrationButton, value);
        }
        #endregion

        #region Icons

        private string _startAndStopMetronomeButtonIcon = Icons.PlayIcon;

        public string StartAndStopMetronomeButtonIcon
        {
            get => _startAndStopMetronomeButtonIcon;
            set => SetProperty(ref _startAndStopMetronomeButtonIcon, value);
        }

        private int icon_x;

        public int IconX
        {
            get => icon_x;
            set => SetProperty(ref icon_x, value);
        }

        #endregion

        public ICommand MinusOneBpmBatton { get; }
        public ICommand PlusOneBpmButton { get; }
        public ICommand StartAndStopMetronomeButton { get; }
        public ICommand OnAndOffSoundButton { get; }
        public ICommand OnAndOffVibrationButton { get; }
        public ICommand EntryReturnCommand { get; }

        public MetronomTabPageViewModel()
        {
            MinusOneBpmBatton = new DelegateCommand(minusOneBpmBatton);
            PlusOneBpmButton = new DelegateCommand(plusOneBpmButton);
            StartAndStopMetronomeButton = new DelegateCommand(startAndStopMetronomeButton);
            OnAndOffSoundButton = new DelegateCommand(onAndOffSoundButton);
            OnAndOffVibrationButton = new DelegateCommand(onAndOffVibrationButton);
            EntryReturnCommand = new DelegateCommand(entryReturnCommand);
            metronome = new Metronome();
        }
        private void minusOneBpmBatton() { if(Bpm > 10) Bpm--; }
        private void plusOneBpmButton() { if (Bpm < 300) Bpm++; }
        private void startAndStopMetronomeButton()
        {
            if(!StartOn)
            {
                BackgroundColorStartAndStopMetronomeButton = Colors.Active;
                TextStartAndStopMetronomeButton = "Stop";
                StartAndStopMetronomeButtonIcon = Icons.StopIcon;
                StartOn = true;
                if (Bpm > 300) Bpm = 300;
                if (Bpm < 10) Bpm = 10;
                UpdateOn = true;
                _ = Update();
                if (metronome != null) metronome.Play();
            }
            else
            {
                BackgroundColorStartAndStopMetronomeButton = Colors.Passive;
                TextStartAndStopMetronomeButton = "Start";
                StartOn = false;
                StartAndStopMetronomeButtonIcon = Icons.PlayIcon;
                if (metronome != null) metronome.Stop();
                UpdateOn = false;
            }
        }
        private void onAndOffSoundButton()
        {
            if(!SoundOn)
            {
                BackgroundColorOnAndOffSoundButton = Colors.Active;
                SoundOn = true;
                TextOnAndOffSoundButton = "On";
            }
            else
            {
                BackgroundColorOnAndOffSoundButton = Colors.Passive;
                SoundOn = false;
                TextOnAndOffSoundButton = "Off";
            }
        }
        private void onAndOffVibrationButton()
        {
            if(!VibroOn)
            {
                BackgroundColorOnAndOffVibrationButton = Colors.Active;
                VibroOn = true;
                TextOnAndOffVibrationButton = "On";
            }
            else
            {
                BackgroundColorOnAndOffVibrationButton = Colors.Passive;
                VibroOn = false;
                TextOnAndOffVibrationButton = "Off";
            }
        }

        private void entryReturnCommand()
        {
            if (Bpm < 10) Bpm = 10;
            if (Bpm > 300) Bpm = 300;
        }

        private async Task Update()
        {
            while (UpdateOn)
            {
                await Task.Run(async () =>
                {
                    FromSecender = 60000 / Bpm;
                    if (metronome != null) metronome.Update(FromSecender, SoundOn, VibroOn);
                    await Task.Delay(250).ConfigureAwait(false);
                });
            }
        }
    }
}
