using HexaEngine.Core.Extensions;
using HexaEngine.Core.IO.Components;
using System;
using System.Collections.Generic;
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
            fs.Close();
            return new HexaEngineArchive(archive, table);
        }

        public static HexaEngineArchive Pack(FileInfo archive, Dictionary<string, FileInfo> files)
        {
            long pointerNow = 0;
            FileTable table = new FileTable();
            foreach (var file in files)
            {
                FileTableEntry entry = new FileTableEntry(table, pointerNow, file.Key);
                table.Add(entry);
                pointerNow += file.Value.Length;
            }

            using var fs = archive.Open(FileMode.Create);
            table.Write(fs);
            foreach (var file in files)
            {
                using var fs1 = file.Value.OpenRead();
                fs1.CopyTo(fs);
                fs1.Close();
            }
            fs.Close();
            return new HexaEngineArchive(archive, table);
        }

        public FileTable FileTable { get; private set; }

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
                fs.Close();
                return new MemoryStream(buffer);
            }
            else
            {
                var end = FileTable.TableEntries[next].AbsolutePointer - fs.Position;
                byte[] buffer = new byte[end];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                return new MemoryStream(buffer);
            }
        }

        public Dictionary<string, FileInfo> Extract(DirectoryInfo directory)
        {
            Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();
            foreach (var entry in FileTable.TableEntries)
            {
                FileInfo file = new FileInfo(directory.FullName + "/" + entry.VirtualPath);
                file.Directory.Create();
                using var fs = file.Create();
                GetFile(entry.VirtualPath).CopyTo(fs);
                fs.Close();
                files.Add(entry.VirtualPath, file);
            }
            return files;
        }

        public void Append(string virtualPath, FileInfo file)
        {
            using var fs = Archive.Open(FileMode.Open);
            FileTableEntry entry = new FileTableEntry(FileTable, fs.Length + 1 - FileTable.TableEndPointer, virtualPath);
            FileTable.Add(entry);
            fs.ExpandStream(FileTable.AbsoluteTableEndPointer, entry.EntrySize);
            FileTable.Write(fs);
            using var srcFs = file.Open(FileMode.Open);
            fs.ExpandStream(fs.Length, srcFs.Length);
            fs.Position = entry.AbsolutePointer;
            srcFs.CopyTo(fs);
            srcFs.Close();
            fs.Close();
        }

        public void Remove(FileTableEntry entry)
        {
            using var fs = Archive.Open(FileMode.Open);
            var count = (FileTable.TableEntries.GetNext(entry)?.AbsolutePointer ?? fs.Length) - entry.AbsolutePointer;
            fs.ShrinkStream(entry.TablePointer, entry.EntrySize);
            FileTable.Remove(entry, count);
            fs.ShrinkStream(entry.AbsolutePointer, count);
            FileTable.Write(fs);
            fs.Close();
        }

        /// <summary>
        /// Updates the archive
        /// </summary>
        /// <param name="files"></param>
        public void Update(Dictionary<string, FileInfo> files)
        {
            var tempDir = GetTempDir();
            var tempfiles = Extract(tempDir);
            tempDir.Delete(true);

            foreach (var deleted in Except(tempfiles, files))
            {
                var entry = FileTable.TableEntries.FirstOrDefault(x => x.VirtualPath == deleted.Key);
                Remove(entry);
            }

            foreach (var added in Except(files, tempfiles))
            {
                Append(added.Key, added.Value);
            }
        }

        public static Dictionary<string, FileInfo> Except(Dictionary<string, FileInfo> a, Dictionary<string, FileInfo> b)
        {
            Dictionary<string, FileInfo> output = new Dictionary<string, FileInfo>();
            foreach (var ax in a)
            {
                if (!b.ContainsKey(ax.Key))
                {
                    output.Add(ax.Key, ax.Value);
                }
            }

            return output;
        }

        public static string GetRelativePath(string from, string to)
        {
            return to.Replace(from, "");
        }

        public static DirectoryInfo GetTempDir()
        {
            DirectoryInfo info = new DirectoryInfo(Path.GetTempPath() + Guid.NewGuid());
            info.Create();
            return info;
        }

        public static FileInfo GetTempFile()
        {
            FileInfo info = new FileInfo(Path.GetTempPath() + Guid.NewGuid());
            return info;
        }
    }
}