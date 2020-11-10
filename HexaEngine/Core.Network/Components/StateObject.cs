using System.Collections.Generic;
using System.Net.Sockets;

namespace HexaEngine.Core.Network.Components
{
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;

        // Client worker.
        public SocketHandler networkHandler = default;

        // Size of receive buffer.
        public const int BufferSize = 256;

        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];

        public List<byte> data = new List<byte>();
    }
}