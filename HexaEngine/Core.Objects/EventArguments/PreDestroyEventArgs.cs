using System;

namespace HexaEngine.Core.Objects.EventArguments
{
    public class PreDestroyEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}