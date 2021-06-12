using HexaFramework.Models.ObjLoader.Loader.Data;
using HexaFramework.Models.ObjLoader.Loader.Data.DataStore;
using HexaFramework.Models.ObjLoader.Loader.TypeParsers.Interfaces;

namespace HexaFramework.Models.ObjLoader.Loader.TypeParsers
{
    public class GroupParser : TypeParserBase, IGroupParser
    {
        private readonly IGroupDataStore _groupDataStore;

        public GroupParser(IGroupDataStore groupDataStore)
        {
            _groupDataStore = groupDataStore;
        }

        protected override string Keyword
        {
            get { return "g"; }
        }

        public override void Parse(string line)
        {
            _groupDataStore.PushGroup(line);
        }
    }
}