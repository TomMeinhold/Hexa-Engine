using System.Collections.Generic;
using HexaFramework.Models.ObjLoader.Loader.Data.Elements;
using HexaFramework.Models.ObjLoader.Loader.Data.VertexData;

namespace HexaFramework.Models.ObjLoader.Loader.Data.DataStore
{
    public interface IDataStore
    {
        IList<Vertex> Vertices { get; }
        IList<Texture> Textures { get; }
        IList<Normal> Normals { get; }
        IList<Material> Materials { get; }
        IList<Group> Groups { get; }
    }
}