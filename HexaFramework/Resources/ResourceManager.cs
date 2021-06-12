using HexaFramework.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HexaFramework.Resources
{
    public class ResourceManager
    {
        private readonly Dictionary<ResourceState, Texture> textures = new();
        private readonly Dictionary<ResourceState, Model> models = new();
        private readonly Dictionary<ResourceState, Sound> sounds = new();

        public ResourceManager(DeviceManager manager)
        {
            Manager = manager;
        }

        public DeviceManager Manager { get; }

        public Texture LoadTexture(string path)
        {
            var path1 = new FileInfo(path).FullName;
            if (textures.Any(x => x.Key.Path == path1))
            {
                return textures.First(x => x.Key.Path == path1).Value;
            }
            else
            {
                Texture texture = new();
                texture.Load(Manager.ID3D11Device, path1);
                textures.Add(new ResourceState() { Loaded = true, Path = path1 }, texture);
                return texture;
            }
        }

        public Model LoadModel(string path)
        {
            var path1 = new FileInfo(path).FullName;
            if (models.Any(x => x.Key.Path == path1))
            {
                return models.First(x => x.Key.Path == path1).Value;
            }
            else
            {
                Model model = new();
                model.Load(Manager, path1);
                models.Add(new ResourceState() { Loaded = true, Path = path1 }, model);
                return model;
            }
        }

        public Model LoadModelObj(string path)
        {
            var path1 = new FileInfo(path).FullName;
            if (models.Any(x => x.Key.Path == path1))
            {
                return models.First(x => x.Key.Path == path1).Value;
            }
            else
            {
                Model model = new();
                model.LoadObj(Manager, path1);
                models.Add(new ResourceState() { Loaded = true, Path = path1 }, model);
                return model;
            }
        }

        public Sound LoadSound(string path)
        {
            var path1 = new FileInfo(path).FullName;
            if (sounds.Any(x => x.Key.Path == path1))
            {
                return sounds.First(x => x.Key.Path == path1).Value;
            }
            else
            {
                Sound sound = new();
                sound.LoadAudioFile(Manager.AudioManager, path1);
                sounds.Add(new ResourceState() { Loaded = true, Path = path1 }, sound);
                return sound;
            }
        }
    }
}