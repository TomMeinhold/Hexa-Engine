using System;

namespace HexaEngine.Core.Input.Component
{
    public struct MouseUpdatePackage
    {
        public MouseUpdatePackage(MouseState mouseState, MouseUpdate mouseUpdate)
        {
            MouseState = mouseState ?? throw new ArgumentNullException(nameof(mouseState));
            MouseUpdate = mouseUpdate;
        }

        public MouseState MouseState { get; set; }

        public MouseUpdate MouseUpdate { get; set; }
    }
}