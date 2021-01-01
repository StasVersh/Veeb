using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Veeb.Models
{
    public class Metronome
    {
        private double fromeSecender;
        private bool SoundButtOn;
        private bool VibroButtOn;
        private bool TimerOn = false;
        private ISimpleAudioPlayer player;

        public string SoundSelect { get; set; }

        public void Play()
        {
            GC.Collect();
            player = CrossSimpleAudioPlayer.Current;
            player.Load("tic_2_sound.mp3");
            TimerOn = true;
            Task.Run(async () => await Timer().ConfigureAwait(false))
                    .ContinueWith(x => Stop()).ConfigureAwait(false);
        }

        public void Stop()
        {
            TimerOn = false;
        }

        public void Update(double FromSecender, bool SoundOn, bool VibroOn)
        {
            fromeSecender = FromSecender;
            SoundButtOn = SoundOn;
            VibroButtOn = VibroOn;
        }

        private async Task Timer()
        {
            while (TimerOn)
            {
                await Task.Run(async () =>
                {
                    if (SoundButtOn) await Task.Run(async () => SoundPlay());
                    if (VibroButtOn) await Task.Run(async () => VibroPlay());
                    await Task.Delay((int)fromeSecender).ConfigureAwait(false);
                });
            }
        }

        private void SoundPlay()
        {
            player.Play();
        }

        private void VibroPlay()
        {
            var derection = TimeSpan.FromMilliseconds(fromeSecender / 4);
            if (fromeSecender > 500) derection = TimeSpan.FromMilliseconds(125);
            Vibration.Vibrate(derection);

        }
    }
}
