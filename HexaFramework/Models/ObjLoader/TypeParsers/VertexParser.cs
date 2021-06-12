using HexaFramework.Models.ObjLoader.Loader.Common;
using HexaFramework.Models.ObjLoader.Loader.Data.DataStore;
using HexaFramework.Models.ObjLoader.Loader.Data.VertexData;
using HexaFramework.Models.ObjLoader.Loader.TypeParsers.Interfaces;
using System;

namespace HexaFramework.Models.ObjLoader.Loader.TypeParsers
{
    public class VertexParser : TypeParserBase, IVertexParser
    {
        private readonly IVertexDataStore _vertexDataStore;

        public VertexParser(IVertexDataStore vertexDataStore)
        {
            _vertexDataStore = vertexDataStore;
        }

        protected override string Keyword
        {
            get { return "v"; }
        }

        public override void Parse(string line)
        {
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var x = parts[0].ParseInvariantFloat();
            var y = parts[1].ParseInvariantFloat();
            var z = parts[2].ParseInvariantFloat();

            var vertex = new Vertex(x, y, z);
            _vertexDataStore.AddVertex(vertex);
        }
    }
}