using System.Threading.Tasks;
using Android.Media;
using System.Text;

namespace Veeb.Droid
{
    public class AudioService : IAudioService
    {
        private AudioTrack _audioTrack;

        public int SamplingRate
        {
            get { return 44100; }
        }

        public int MinBufferSize { get; private set; }

        public bool IsPlaying
        {
            get { return _audioTrack.PlayState == PlayState.Playing; }
        }

        public AudioService()
        {
            MinBufferSize = AudioTrack.GetMinBufferSize(SamplingRate, ChannelOut.Mono, Android.Media.Encoding.Pcm16bit);
        }

        public void StartPlaying()
        {
            _audioTrack = new AudioTrack(Stream.Music, SamplingRate, ChannelOut.Mono, Android.Media.Encoding.Pcm16bit, 2 * MinBufferSize, AudioTrackMode.Stream);
            _audioTrack.Play();
        }

        public void StopPlaying()
        {
            if (_audioTrack != null)
            {
                _audioTrack.Stop();
                _audioTrack.Release();
                _audioTrack = null;
            }
        }

        public async Task PlayAsync(byte[] sound, int length)
        {
            if (_audioTrack != null && IsPlaying)
            {
                await _audioTrack.WriteAsync(sound, 0, sound.Length).ConfigureAwait(false);
            }
        }
    }
}