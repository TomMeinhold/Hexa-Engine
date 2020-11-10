using HexaEngine.Core.IO.Components;
using System.IO;
using System.Linq;

namespace HexaEngine.Core.IO
{
    public class HexaEngineArchive
    {
        private HexaEngineArchive(FileInfo archive, FileTable table)
        {
            Archive = archive;
            FileTable = table;
        }

        public static HexaEngineArchive Load(FileInfo archive)
        {
            using var fs = archive.Open(FileMode.Open);
            FileTable table = new FileTable(fs);
            table.ToString();

            return new HexaEngineArchive(archive, table);
        }

        public static HexaEngineArchive Create(FileInfo archive, DirectoryInfo directory)
        {
            long pointerNow = 0;
            FileTable table = new FileTable();
            foreach (FileInfo file in directory.GetFiles("*.*", SearchOption.AllDirectories))
            {
                FileTableEntry entry = new FileTableEntry(table, pointerNow, GetRelativePath(directory.FullName, file.FullName));
                table.Add(entry);
                pointerNow += file.Length;
            }

            using var fs = archive.Open(FileMode.Create);
            table.Write(fs);
            foreach (FileInfo file in directory.GetFiles("*.*", SearchOption.AllDirectories))
            {
                using var fs1 = file.OpenRead();
                fs1.CopyTo(fs);
                fs1.Close();
            }
            fs.Close();
            return new HexaEngineArchive(archive, table);
        }

        public FileTable FileTable { get; }

        public FileInfo Archive { get; }

        public MemoryStream GetFile(string path)
        {
            var fs = Archive.OpenRead();
            var entry = FileTable.TableEntries.FirstOrDefault(x => x.VirtualPath == path);
            fs.Position = entry.AbsolutePointer;
            var next = FileTable.TableEntries.FindIndex(x => x == entry) + 1;
            if (FileTable.TableEntries.Count == next)
            {
                byte[] buffer = new byte[fs.Length - fs.Position];
                fs.Read(buffer, 0, buffer.Length);
                return new MemoryStream(buffer);
            }
            else
            {
                var end = FileTable.TableEntries[next].AbsolutePointer - fs.Position;
                byte[] buffer = new byte[end];
                fs.Read(buffer, 0, buffer.Length);
                return new MemoryStream(buffer);
            }
        }

        private static string GetRelativePath(string from, string to)
        {
            return to.Replace(from, "");
        }
    }
}