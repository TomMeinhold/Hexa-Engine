using HexaFramework.Models.ObjLoader.Loader.Data.VertexData;

namespace HexaFramework.Models.ObjLoader.Loader.Data.DataStore
{
    public interface IVertexDataStore
    {
        void AddVertex(Vertex vertex);
    }
}