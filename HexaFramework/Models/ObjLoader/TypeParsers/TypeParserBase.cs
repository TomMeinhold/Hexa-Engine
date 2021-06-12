using HexaFramework.Models.ObjLoader.Loader.Common;
using HexaFramework.Models.ObjLoader.Loader.TypeParsers.Interfaces;

namespace HexaFramework.Models.ObjLoader.Loader.TypeParsers
{
    public abstract class TypeParserBase : ITypeParser
    {
        protected abstract string Keyword { get; }

        public bool CanParse(string keyword)
        {
            return keyword.EqualsOrdinalIgnoreCase(Keyword);
        }

        public abstract void Parse(string line);
    }
}