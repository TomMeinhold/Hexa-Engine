using HexaEngine.Core.Network.Components;
using HexaEngine.Core.Network.Enums;
using System;
using System.Net;

namespace HexaEngine.Core.Network
{
    public class Client : IDisposable
    {
        public Client()
        {
        }

        public IPAddress IP { get; private set; }

        public int Port { get; private set; }

        public SocketHandler SocketHandler { get; private set; }

        public void Connect(IPAddress ip, int port)
        {
            SocketHandler = new SocketHandler(SocketHandlerMode.Connect, ip, port);
        }

        public void Disconnect()
        {
            SocketHandler?.Disconnect();
        }

        public void Dispose()
        {
            SocketHandler?.Disconnect();
            SocketHandler?.Dispose();
        }
    }
}