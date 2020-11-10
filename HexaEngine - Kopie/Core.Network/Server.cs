using HexaEngine.Core.Network.Components;
using HexaEngine.Core.Network.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HexaEngine.Core.Network
{
    public class Server : IDisposable
    {
        public Server()
        {
        }

        ~Server()
        {
            Dispose(disposing: false);
        }

        private bool disposedValue;

        public event EventHandler<SocketHandler> ClientConnected;

        public event EventHandler<SocketHandler> ClientDisconnected;

        public event EventHandler<Package> ReceivedPackage;

        public event EventHandler<Server> ServerStarted;

        public event EventHandler<Server> ServerClosing;

        public event EventHandler<Server> ServerClosed;

        public IPAddress IP { get; private set; }
        public int Port { get; private set; }
        public List<SocketHandler> SocketHandlers { get; } = new List<SocketHandler>();

        public SocketHandler SocketHandler { get; private set; }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Start(IPAddress ip, int port)
        {
            IP = ip;
            Port = port;
            SocketHandler = new SocketHandler(Enums.SocketHandlerMode.Listen, ip, port);
            SocketHandler.ServerToClientConnected += SocketHandler_ServerToClientConnected;
            ServerStarted?.Invoke(this, this);
        }

        private void SocketHandler_ServerToClientConnected(object sender, SocketHandler e)
        {
            SocketHandlers.Add(e);
            e.UnsafeDisconnected += ClientDisconnected;
            e.Disconnected += ClientDisconnected;
            e.OnReceive += ReceivedPackage;
            ClientConnected?.Invoke(this, e);
        }

        public void Close()
        {
            ServerClosing?.Invoke(this, this);
            Dispose();
            ServerClosed?.Invoke(this, this);
        }

        public void Broadcast(Package package)
        {
            SocketHandlers.ForEach(x =>
            {
                x.Send(package);
            });
        }

        public void Broadcast(Package package, params SocketHandler[] exclude)
        {
            SocketHandlers.ForEach(x =>
            {
                if (!exclude.Contains(x))
                {
                    x.Send(package);
                }
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SocketHandlers.ForEach((x) => x.Disconnect());
                    SocketHandlers.ForEach((x) => x.Dispose());
                    SocketHandler.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}