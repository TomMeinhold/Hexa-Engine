using SharpDX.XAudio2;

namespace HexaEngine.Core.Audio
{
    public partial class AudioSystem
    {
        public XAudio2 XAudio2;

        public MasteringVoice MasteringVoice;

        public AudioSystem()
        {
            XAudio2 = new XAudio2();
            MasteringVoice = new MasteringVoice(XAudio2);
        }

        public void Dispose()
        {
            MasteringVoice.Dispose();
            XAudio2.Dispose();
        }
    }
}
