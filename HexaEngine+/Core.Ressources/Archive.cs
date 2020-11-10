using HexaEngine.Core.IO;
using HexaEngine.Core.Render.Components;
using SharpDX.Direct2D1;
using System.Collections.Generic;
using System.Linq;

namespace HexaEngine.Core.Ressources
{
    public class Archive
    {
        public Archive(HexaEngineArchive archive)
        {
            HexaEngineArchive = archive;
        }

        public static List<Sound> Sounds { get; } = new List<Sound>();

        public static List<Sprite> Sprites { get; } = new List<Sprite>();

        public static List<Texture> Textures { get; } = new List<Texture>();

        public static List<Bitmap1> Bitmaps { get; } = new List<Bitmap1>();

        public HexaEngineArchive HexaEngineArchive { get; }

        public void Load(RessourceType ressouce, string name)
        {
        }

        public bool Contains(RessourceType ressouce, string name)
        {
            return ressouce switch
            {
                RessourceType.Texture => !(GetTexture(name) is null),
                RessourceType.Sprite => !(GetSprite(name) is null),
                RessourceType.Sound => !(GetSound(name) is null),
                _ => false,
            };
        }

        public Texture GetTexture(string name)
        {
            return Textures.FirstOrDefault(x => x.Name == name);
        }

        public Sprite GetSprite(string name)
        {
            return Sprites.FirstOrDefault(x => x.Name == name);
        }

        public Sound GetSound(string name)
        {
            return Sounds.FirstOrDefault(x => x.Name == name);
        }
    }
}