using System.IO;

namespace HexaFramework.Models.ObjLoader.Loader.Loaders
{
    public interface IMaterialLibraryLoader
    {
        void Load(Stream lineStream);
    }
}