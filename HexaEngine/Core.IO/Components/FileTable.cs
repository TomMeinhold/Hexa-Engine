using System;
using System.Collections.Generic;
using System.IO;

namespace HexaEngine.Core.IO.Components
{
    public class FileTable
    {
        public const int PointerSize = 8;

        public FileTable()
        {
        }

        public FileTable(Stream stream)
        {
            TableEndPointer = BitConverter.ToInt64(ReadStream(stream, PointerSize), 0);
            while (stream.Position < TableEndPointer + PointerSize)
            {
                TableEntries.Add(new FileTableEntry(this, stream));
            }
        }

        public long TableEndPointer { get; set; }

        public long AbsoluteTableEndPointer { get => TableEndPointer + PointerSize; }

        public List<FileTableEntry> TableEntries { get; } = new List<FileTableEntry>();

        public void Add(FileTableEntry entry)
        {
            TableEntries.Add(entry);
            TableEndPointer += entry.EntrySize;
        }

        public void Remove(FileTableEntry entry, long size)
        {
            var index = TableEntries.IndexOf(entry);
            for (int i = 0; i < TableEntries.Count;)
            {
                if (i > index)
                {
                    TableEntries[i].Pointer -= size;
                }
                i++;
            }
            TableEntries.Remove(entry);
            TableEndPointer -= entry.EntrySize;
        }

        public void Write(Stream stream)
        {
            stream.Position = 0;
            stream.Write(BitConverter.GetBytes(TableEndPointer), 0, PointerSize);
            foreach (FileTableEntry entry in TableEntries)
            {
                entry.Write(stream);
            }
        }

        private byte[] ReadStream(Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}