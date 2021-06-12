using HexaFramework.Audio;
using System.IO;
using System.Numerics;
using Vortice.Multimedia;
using Vortice.XAudio2;

namespace HexaFramework.Resources
{
    public class Sound : Resource
    {
        // Variables
        private string chunkId;

        private int chunkSize;
        private string format;
        private string subChunkId;
        private int subChunkSize;
        private WaveFormatEncoding audioFormat;
        private short numChannels;
        private int sampleRate;
        private int bytesPerSecond;
        private short blockAlign;
        private short bitsPerSample;
        private string dataChunkId;
        private int dataSize;

        public IXAudio2SourceVoice SourceVoice { get; private set; }

        public AudioBuffer Buffer { get; private set; }
        public AudioManager Manager { get; private set; }

        // Constructor
        public Sound()
        {
        }

        public static Sound Load(ResourceManager manager, string path)
        {
            return manager.LoadSound(path);
        }

        // Virtual Methods
        internal bool LoadAudioFile(AudioManager manager, string audioFile)
        {
            try
            {
                Manager = manager;
                // Open the wave file in binary.
                BinaryReader reader = new(File.OpenRead(audioFile));

                // Read in the wave file header.
                chunkId = new string(reader.ReadChars(4));
                chunkSize = reader.ReadInt32();
                format = new string(reader.ReadChars(4));
                subChunkId = new string(reader.ReadChars(4));
                subChunkSize = reader.ReadInt32();
                audioFormat = (WaveFormatEncoding)reader.ReadInt16();
                numChannels = reader.ReadInt16();
                sampleRate = reader.ReadInt32();
                bytesPerSecond = reader.ReadInt32();
                blockAlign = reader.ReadInt16();
                bitsPerSample = reader.ReadInt16();
                dataChunkId = new string(reader.ReadChars(4));
                dataSize = reader.ReadInt32();

                // Check that the chunk ID is the RIFF format
                // and the file format is the WAVE format
                // and sub chunk ID is the fmt format
                // and the audio format is PCM
                // and the wave file was recorded in stereo format
                // and at a sample rate of 44.1 KHz
                // and at 16 bit format
                // and there is the data chunk header.
                // Otherwise return false.
                // modified in Tutorial 31 for 3D Sound loading stereo files in a mono Secondary buffer.
                if (chunkId != "RIFF" || format != "WAVE" || subChunkId.Trim() != "fmt" || audioFormat != WaveFormatEncoding.Pcm || numChannels > 2 || sampleRate != 44100 || bitsPerSample != 16 || dataChunkId != "data")
                    return false;

                // Read in the wave file data into the temporary buffer.
                byte[] waveData = reader.ReadBytes(dataSize);

                // Close the reader
                reader.Close();

                var waveFormat = new WaveFormat(44100, 16, 2);

                var voice = SourceVoice = Manager.IXAudio2.CreateSourceVoice(waveFormat);
                AudioBuffer audioBuffer = Buffer = new(waveData, BufferFlags.EndOfStream);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Play(int volume)
        {
            try
            {
                SourceVoice.SubmitSourceBuffer(Buffer);
                SourceVoice.SetVolume(volume);
                SourceVoice.Start(0);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}