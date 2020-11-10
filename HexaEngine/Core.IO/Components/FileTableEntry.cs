using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HexaEngine.Core.IO.Components
{
    public class FileTableEntry
    {
        public FileTableEntry(FileTable table, long pointer, string virtualPath)
        {
            Table = table;
            Pointer = pointer;
            VirtualPath = virtualPath;
            EntrySize = virtualPath.Length + PointerSize + 1;
        }

        public FileTableEntry(FileTable table, Stream stream)
        {
            Table = table;
            Pointer = BitConverter.ToInt64(ReadStream(stream, PointerSize), 0);
            VirtualPath = Encoding.UTF8.GetString(ReadToControlSymbol(stream, 3));
            EntrySize = VirtualPath.Length + PointerSize + 1;
        }

        public FileTable Table { get; }

        public const int PointerSize = 8;

        public long Pointer { get; }

        public long AbsolutePointer { get => Pointer + Table.AbsoluteTableEndPointer; }

        public string VirtualPath { get; }

        public string FileName { get => Path.GetFileName(VirtualPath); }

        public byte[] PathEncoded { get => Encoding.UTF8.GetBytes(VirtualPath); }

        public int EntrySize { get; }

        public void Write(Stream stream)
        {
            stream.Write(BitConverter.GetBytes(Pointer), 0, PointerSize);
            stream.Write(PathEncoded, 0, PathEncoded.Length);
            stream.Write(new byte[] { 3 }, 0, 1);
        }

        public byte[] ReadToControlSymbol(Stream stream, byte symbol)
        {
            List<byte> output = new List<byte>();
            byte current = 0;
            while (current != symbol)
            {
                byte[] buffer = new byte[1];
                stream.Read(buffer, 0, buffer.Length);
                current = buffer[0];
                output.Add(current);
            }
            output.RemoveAt(output.Count - 1);
            return output.ToArray();
        }

        private byte[] ReadStream(Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}