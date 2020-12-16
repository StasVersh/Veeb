using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Veeb.Models;
using Veeb.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace Veeb.ViewModels
{
    public class MetronomTabPageViewModel : BindableBase
    {
        #region Vars
        private double bpm = 120;
        private double fromSecender;
        private bool start_or_stop_metronome_button = false;
        private bool on_or_off_sound_button = false;
        private bool on_or_off_vibration_button = false;
        private bool startAndStopTapingTempo = false; 
        private  player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
        //private bool OnOrOffButtonTapingTempo = false;
        private int sheare = 0;
        //private State state;
        //Stopwatch stopWatch = new Stopwatch();
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
        private string _roundIconOne = Icons.GrayRoundIcon;
        public string RoundIconOne
        {
            get => _roundIconOne;
            set => SetProperty(ref _roundIconOne, value);
        }
        private string _roundIconTwo = Icons.GrayRoundIcon;
        public string RoundIconTwo
        {
            get => _roundIconTwo;
            set => SetProperty(ref _roundIconTwo, value);
        }
        private string _roundIconThree = Icons.GrayRoundIcon;
        public string RoundIconThree
        {
            get => _roundIconThree;
            set => SetProperty(ref _roundIconThree, value);
        }
        private string _roundIconFour = Icons.GrayRoundIcon;
        public string RoundIconFour
        {
            get => _roundIconFour;
            set => SetProperty(ref _roundIconFour, value);
        }
        private string _startAndStopMetronomeButtonIcon = Icons.PlayIcon;
        public string StartAndStopMetronomeButtonIcon
        {
            get => _startAndStopMetronomeButtonIcon;
            set => SetProperty(ref _startAndStopMetronomeButtonIcon, value);
        }
        #endregion

        public ICommand MinusOneBpmBatton { get; }
        public ICommand PlusOneBpmButton { get; }
        public ICommand StartAndStopMetronomeButton { get; }
        public ICommand OnAndOffSoundButton { get; }
        public ICommand OnAndOffVibrationButton { get; }
        public ICommand EntryReturnCommand { get; }
        public ICommand TapingTempoButton { get; }


        public MetronomTabPageViewModel()
        {
            MinusOneBpmBatton = new DelegateCommand(minusOneBpmBatton);
            PlusOneBpmButton = new DelegateCommand(plusOneBpmButton);
            StartAndStopMetronomeButton = new DelegateCommand(startAndStopMetronomeButton);
            OnAndOffSoundButton = new DelegateCommand(onAndOffSoundButton);
            OnAndOffVibrationButton = new DelegateCommand(onAndOffVibrationButton);
            EntryReturnCommand = new DelegateCommand(entryReturnCommand);
            TapingTempoButton = new DelegateCommand(tapingTempoButton);
        }
        private void minusOneBpmBatton() { if(Bpm > 10) Bpm--; }
        private void plusOneBpmButton() { if (Bpm < 300) Bpm++; }
        private void startAndStopMetronomeButton()
        {
            if(!start_or_stop_metronome_button)
            {
                BackgroundColorStartAndStopMetronomeButton = Colors.Active;
                TextStartAndStopMetronomeButton = "Stop";
                StartAndStopMetronomeButtonIcon = Icons.StopIcon;
                if (Bpm > 300) Bpm = 300;
                if (Bpm < 10) Bpm = 10;
                fromSecender = 60000 / Bpm;
                TimerTick();             
                _ = MetronomeTimerAsync();
                start_or_stop_metronome_button = true;
            }
            else
            {
                BackgroundColorStartAndStopMetronomeButton = Colors.Passive;
                TextStartAndStopMetronomeButton = "Start";
                start_or_stop_metronome_button = false;
                StartAndStopMetronomeButtonIcon = Icons.PlayIcon;
                if (sheare == 0) RoundIconOne = Icons.GrayRoundIcon;
                if (sheare == 1) RoundIconTwo = Icons.GrayRoundIcon;
                if (sheare == 2) RoundIconThree = Icons.GrayRoundIcon;
                if (sheare == 3) RoundIconFour = Icons.GrayRoundIcon;
                sheare = 0;
            }
        }
        private void onAndOffSoundButton()
        {
            if(!on_or_off_sound_button)
            {
                BackgroundColorOnAndOffSoundButton = Colors.Active;
                on_or_off_sound_button = true;
                TextOnAndOffSoundButton = "On";
            }
            else
            {
                BackgroundColorOnAndOffSoundButton = Colors.Passive;
                on_or_off_sound_button = false;
                TextOnAndOffSoundButton = "Off";
            }
        }
        private void onAndOffVibrationButton()
        {
            if(!on_or_off_vibration_button)
            {
                BackgroundColorOnAndOffVibrationButton = Colors.Active;
                on_or_off_vibration_button = true;
                TextOnAndOffVibrationButton = "On";
            }
            else
            {
                BackgroundColorOnAndOffVibrationButton = Colors.Passive;
                on_or_off_vibration_button = false;
                TextOnAndOffVibrationButton = "Off";
            }
        }

        private void entryReturnCommand()
        {
            if (Bpm < 10) Bpm = 10;
            if (Bpm > 300) Bpm = 300;
        }

        private void tapingTempoButton()
        {
            if(!startAndStopTapingTempo)
            {
                
            }
            //else
            //{
            //    stopWatch.Stop();
            //    timeSpan = stopWatch.Elapsed;
            //    fromSecender = timeSpan.TotalSeconds;
            //    Bpm = 60 / fromSecender;
            //    stopWatch.Reset();
            //    startAndStopTapingTempo = false;
            //}
        }

        private async Task MetronomeTimerAsync()
        {
            await Task.Run(async () =>
            {
                await Task.Delay((int)fromSecender).ConfigureAwait(false);
                TimerTick();

            });

        }

        private bool TimerTick()
        {
            VisualTick();
            fromSecender = 60000 / Bpm;
            if (on_or_off_vibration_button)
            {
                VibroTick();
            }
            if (on_or_off_sound_button)
            {
                if (start_or_stop_metronome_button)
                {
                    SoundTick();
                }
            }
            if (start_or_stop_metronome_button) _ = MetronomeTimerAsync();
            return start_or_stop_metronome_button;
        }

        private async void VisualTick()
        {
            await Task.Run(() =>
            {
                if (start_or_stop_metronome_button)
                {
                    if (sheare == 0) RoundIconOne = Icons.GreenRoundIcon;
                    else RoundIconOne = Icons.GrayRoundIcon;
                    if (sheare == 1) RoundIconTwo = Icons.BlueRoundIcon;
                    else RoundIconTwo = Icons.GrayRoundIcon;
                    if (sheare == 2) RoundIconThree = Icons.GreenRoundIcon;
                    else RoundIconThree = Icons.GrayRoundIcon;
                    if (sheare == 3) RoundIconFour = Icons.BlueRoundIcon;
                    else RoundIconFour = Icons.GrayRoundIcon;
                    sheare++;
                    if (sheare == 4) sheare = 0;
                }
                else
                {
                    RoundIconOne = Icons.GrayRoundIcon;
                    RoundIconTwo = Icons.GrayRoundIcon;
                    RoundIconThree = Icons.GrayRoundIcon;
                    RoundIconFour = Icons.GrayRoundIcon;
                }
            });  
        }

        private async void SoundTick()
        {
            await Task.Run(() =>
            {
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load("tic_2_sound.mp3");
                player.Play();
            });
        }

        private async void VibroTick()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (start_or_stop_metronome_button)
                    {
                        Vibration.Cancel();
                        var duration2 = TimeSpan.FromMilliseconds(250);
                        Vibration.Vibrate(duration2);
                    }
                }
                catch (FeatureNotSupportedException)
                {
                    TextStartAndStopMetronomeButton = "function does not work";
                }
                catch (Exception)
                {
                    TextStartAndStopMetronomeButton = "error";
                }
            });
        }
    }
}
