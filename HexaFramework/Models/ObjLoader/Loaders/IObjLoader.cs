using System.IO;

namespace HexaFramework.Models.ObjLoader.Loader.Loaders
{
    public interface IObjLoader
    {
        LoadResult Load(Stream lineStream);
    }
}