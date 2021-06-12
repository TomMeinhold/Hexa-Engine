using Vortice.Multimedia;
using Vortice.XAudio2;

namespace HexaFramework.Audio
{
    public class AudioManager
    {
        public X3DAudio X3DAudio { get; } = new(Speakers.All);

        public IXAudio2 IXAudio2 { get; }

        public Listener Listener { get; } = new Listener();

        public IXAudio2MasteringVoice MasteringVoice { get; }

        public AudioManager()
        {
            IXAudio2 = XAudio2.XAudio2Create(ProcessorSpecifier.UseDefaultProcessor);
            MasteringVoice = IXAudio2.CreateMasteringVoice(0, 0, AudioStreamCategory.GameMedia);
        }
    }
}