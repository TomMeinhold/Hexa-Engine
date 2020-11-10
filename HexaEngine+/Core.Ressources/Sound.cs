using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Ressources
{
    public class Sound
    {
        private bool disposedValue;

        private Sound(FileInfo file)
        {
            Name = file.Name.Replace(file.Extension, "");
            using var stream = new SoundStream(file.OpenRead());
            WaveFormat = stream.Format;
            Buffer = new AudioBuffer
            {
                Stream = stream.ToDataStream(),
                AudioBytes = (int)stream.Length,
                Flags = BufferFlags.EndOfStream
            };
            DecodedPacketsInfo = stream.DecodedPacketsInfo;
            stream.Close();
            File = file;
        }

        public FileInfo File { get; }

        public string Name { get; set; }

        public AudioBuffer Buffer { get; set; }

        public uint[] DecodedPacketsInfo { get; set; }

        public WaveFormat WaveFormat { get; set; }

        public SourceVoice SourceVoice { get; set; }

        public bool IsPlaying { get; private set; }

        public bool Repeat { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="file">Only filename not full path</param>
        public static void Load(string file)
        {
            Sound sound = new Sound(new FileInfo(Engine.SoundsPath.FullName + "\\" + file));
            RessourceManager.Sounds.Add(sound);
        }

        public static Sound LoadUnmanaged(string file)
        {
            return new Sound(new FileInfo(Engine.SoundsPath.FullName + "\\" + file));
        }

        public static Sound Get(string file)
        {
            return RessourceManager.GetSound(file);
        }

        public void Unload()
        {
            RessourceManager.Unload(Name, RessourceType.Sound);
        }

        public void Play(XAudio2 xAudio2)
        {
            SourceVoice = new SourceVoice(xAudio2, WaveFormat, true);
            SourceVoice.BufferStart += SourceVoice_BufferStart;
            SourceVoice.BufferEnd += SourceVoice_BufferEnd;
            SourceVoice.SubmitSourceBuffer(Buffer, DecodedPacketsInfo);
            SourceVoice.Start();
        }

        public void PlayDelayed(XAudio2 xAudio2, TimeSpan delay)
        {
            Task.Run(() =>
            {
                Thread.Sleep(delay);
                SourceVoice = new SourceVoice(xAudio2, WaveFormat, true);
                SourceVoice.BufferStart += SourceVoice_BufferStart;
                SourceVoice.BufferEnd += SourceVoice_BufferEnd;
                SourceVoice.SubmitSourceBuffer(Buffer, DecodedPacketsInfo);
                SourceVoice.Start();
            });
        }

        private void SourceVoice_BufferEnd(IntPtr obj)
        {
            IsPlaying = false;
            if (Repeat)
            {
                SourceVoice.SubmitSourceBuffer(Buffer, DecodedPacketsInfo);
            }
        }

        private void SourceVoice_BufferStart(IntPtr obj)
        {
            IsPlaying = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SourceVoice.DestroyVoice();
                    SourceVoice.Dispose();
                }

                disposedValue = true;
            }
        }

        ~Sound()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}