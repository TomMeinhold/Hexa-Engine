using System;

namespace HexaEngine.Core.Physics.Collision
{
    public class OnCollisionEventArgs : EventArgs
    {
        public OnCollisionEventArgs(object @object)
        {
            Object = @object;
        }

        public object Object { get; set; }

        public float PenetrationDepth { get; set; }
    }
}