using HexaEngine.Core.Ressources;
using SharpDX.XAudio2;
using System;
using System.Linq;

namespace HexaEngine.Core.Sounds
{
    public partial class SoundSystem
    {
        public XAudio2 XAudio2;

        public MasteringVoice MasteringVoice;

        public SoundSystem()
        {
            XAudio2 = new XAudio2();
            XAudio2.StartEngine();
            MasteringVoice = new MasteringVoice(XAudio2);
        }

        public void Play(string name)
        {
            RessourceManager.Sounds.FirstOrDefault(x => x.Name == name).Play(XAudio2);
        }

        public void PlayDelayed(string name, TimeSpan delay)
        {
            RessourceManager.Sounds.FirstOrDefault(x => x.Name == name).PlayDelayed(XAudio2, delay);
        }

        public void SetRepeat(string name, bool repeat)
        {
            RessourceManager.Sounds.FindAll(x => x.Name == name).ForEach(x => x.Repeat = repeat);
        }

        public void Dispose()
        {
            MasteringVoice.Dispose();
            XAudio2.StopEngine();
            XAudio2.Dispose();
        }
    }
}