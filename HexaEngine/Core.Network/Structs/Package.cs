using HexaEngine.Core.Network.Components;
using HexaEngine.Core.Network.Enums;
using System;
using System.IO;
using System.Text;

namespace HexaEngine.Core.Network.Structs
{
    public struct Package
    {
        private const int IntByteSize = 4;

        public Package(byte[] data, Command command)
        {
            Data = data;
            Command = command;
        }

        public Package(string str, Command command)
        {
            Data = Encoding.UTF8.GetBytes(str);
            Command = command;
        }

        public Package(Command command)
        {
            Data = Array.Empty<byte>();
            Command = command;
        }

        public byte[] Data { get; private set; }

        public Command Command { get; private set; }

        public static Package Decode(StateObject state)
        {
            Package str = new Package();
            using MemoryStream memory = new MemoryStream(state.data.ToArray());
            byte[] enumBuffer = new byte[IntByteSize];
            byte[] dataBuffer = new byte[memory.Length - IntByteSize];
            memory.Read(enumBuffer, 0, IntByteSize);
            memory.Read(dataBuffer, 0, dataBuffer.Length);
            str.Command = (Command)BitConverter.ToInt32(enumBuffer, 0);
            str.Data = dataBuffer;
            return str;
        }

        public byte[] Encode()
        {
            int size = IntByteSize + Data.Length;
            using MemoryStream memory = new MemoryStream(size);
            memory.Write(BitConverter.GetBytes((int)Command), 0, IntByteSize);
            memory.Write(Data, 0, Data.Length);
            var data = memory.ToArray();
            return data;
        }
    }
}