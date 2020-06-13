using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IPhysicsObjectGrid
    {
        public List<IPhysicsObject> PhysicsObjects { get; }

        public float Mass { get; set; }

        public bool StaticBounds { get; set; }

        public Vector3 MaximumDistance { get; set; }

        public Vector3 MinimumDistance { get; set; }
    }
}