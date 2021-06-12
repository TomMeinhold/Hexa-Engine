using HexaFramework.Models.ObjLoader.Loader.Data;
using HexaFramework.Models.ObjLoader.Loader.Data.DataStore;
using HexaFramework.Models.ObjLoader.Loader.TypeParsers.Interfaces;

namespace HexaFramework.Models.ObjLoader.Loader.TypeParsers
{
    public class UseMaterialParser : TypeParserBase, IUseMaterialParser
    {
        private readonly IElementGroup _elementGroup;

        public UseMaterialParser(IElementGroup elementGroup)
        {
            _elementGroup = elementGroup;
        }

        protected override string Keyword
        {
            get { return "usemtl"; }
        }

        public override void Parse(string line)
        {
            _elementGroup.SetMaterial(line);
        }
    }
}