using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Veeb.Models;
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
        private State state;
        #endregion

        #region Strings
        public string BpmTextEntry
        {
            get => bpm.ToString("###");
            set
            {
                if (double.TryParse(value, out double bpm) && bpm > 9 && bpm < 301)
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
        #endregion
        
        //Colors
        private Color _backgroundColorStartAndStopMetronomeButton = Color.FromArgb(110, 147, 214);
        public Color BackgroundColorStartAndStopMetronomeButton
        {
            get => _backgroundColorStartAndStopMetronomeButton;
            set => SetProperty(ref _backgroundColorStartAndStopMetronomeButton, value);
        }
        private Color _backgroundColorOnAndOffSoundButton = Color.FromArgb(110, 147, 214);  
        public Color BackgroundColorOnAndOffSoundButton
        {
            get => _backgroundColorOnAndOffSoundButton;
            set => SetProperty(ref _backgroundColorOnAndOffSoundButton, value);
        }
        private Color _backgroundColorOnAndOffVibrationButton = Color.FromArgb(110, 147, 214);
        public Color BackgroundColorOnAndOffVibrationButton
        {
            get => _backgroundColorOnAndOffVibrationButton;
            set => SetProperty(ref _backgroundColorOnAndOffVibrationButton, value);
        }

        //Buttons
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
        }

        private void minusOneBpmBatton() { bpm--; }
        private void plusOneBpmButton() { bpm++; }
        private void startAndStopMetronomeButton()
        {
            if(start_or_stop_metronome_button == false)
            {
                Tick();
                MetronomeTimer();              
            }
            else
            {
                BackgroundColorStartAndStopMetronomeButton = Color.FromArgb(110, 147, 214);
                TextStartAndStopMetronomeButton = "Start";
                start_or_stop_metronome_button = false;
            }
        }
        private void onAndOffSoundButton()
        {
            if(!on_or_off_sound_button)
            {
                BackgroundColorOnAndOffSoundButton = Color.FromArgb(99, 165, 131);
                on_or_off_sound_button = true;
                TextOnAndOffSoundButton = "On";
            }
            else
            {
                BackgroundColorOnAndOffSoundButton = Color.FromArgb(110, 147, 214);
                on_or_off_sound_button = false;
                TextOnAndOffSoundButton = "Off";
            }
        }
        private void onAndOffVibrationButton()
        {
            if(!on_or_off_vibration_button)
            {
                BackgroundColorOnAndOffVibrationButton = Color.FromArgb(99, 165, 131);
                on_or_off_vibration_button = true;
                TextOnAndOffVibrationButton = "On";
            }
            else
            {
                BackgroundColorOnAndOffVibrationButton = Color.FromArgb(110, 147, 214);
                on_or_off_vibration_button = false;
                TextOnAndOffVibrationButton = "Off";
            }
        }

        private void entryReturnCommand()
        {
            
        }

        private void MetronomeTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(fromSecender), Tick);
        }

        private bool Tick()
        {
            if (start_or_stop_metronome_button == false)
            {
                state = State.None;
            }
            else
            {
                state = state == State.Fourth ? State.First : ++state;
            }

            //Vibration
            if (on_or_off_vibration_button)
            {
                try
                {
                    if (start_or_stop_metronome_button)
                    {
                        Vibration.Cancel();
                        double timeSpan;
                        if ((fromSecender / 3) > (0.5 / 3)) timeSpan = 0.5 / 3;
                        else timeSpan = fromSecender / 3;
                        var duration2 = TimeSpan.FromSeconds(timeSpan);
                        fromSecender = 1 / (bpm / 60);
                        Vibration.Vibrate(duration2);
                    }
                }
                catch (FeatureNotSupportedException ex)
                {
                    TextStartAndStopMetronomeButton = "function does not work";
                }
                catch (Exception ex)
                {
                    TextStartAndStopMetronomeButton = "error";
                }
            }

            //Sound
            if (on_or_off_sound_button)
            {
                if (start_or_stop_metronome_button)
                {
                    DependencyService.Get<IAudioService>().PlayAudioFile("tic_1_sound.mp3");
                }
            }
            return start_or_stop_metronome_button;
        }
    }
}
