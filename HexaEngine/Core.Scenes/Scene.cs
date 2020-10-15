﻿using HexaEngine.Core.Objects.Interfaces;
using System.Collections.Generic;

namespace HexaEngine.Core.Scenes
{
    public class Scene
    {
        internal List<IBaseObject> Objects { get; } = new List<IBaseObject>();

        public void Add(IBaseObject baseObject)
        {
            lock (Objects)
            {
                Objects.Add(baseObject);
            }
        }

        public void Remove(IBaseObject baseObject)
        {
            lock (Objects)
            {
                Objects.Remove(baseObject);
            }
        }
    }
}