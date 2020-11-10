using HexaEngine.Core.Extensions;
using HexaEngine.Core.Network.Enums;
using HexaEngine.Core.Network.Structs;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Network.Components
{
    public class SocketHandler : IDisposable
    {
        private readonly ManualResetEvent DisconnectDone = new ManualResetEvent(false);
        private readonly ManualResetEvent ConnectDone = new ManualResetEvent(false);
        private readonly ManualResetEvent ReceiveDone = new ManualResetEvent(false);
        private readonly ManualResetEvent SendDone = new ManualResetEvent(false);
        private readonly ManualResetEvent ListenDone = new ManualResetEvent(false);
        private Socket socket;
        private Package sendPackage;

        private readonly Task listenerTask;
        private readonly Task receiveTask;
        private bool disposing;
        private bool disposedValue;

        public event EventHandler<Package> OnReceive;

        public event EventHandler<Package> OnSend;

        public event EventHandler<SocketHandler> Disconnected;

        public event EventHandler<SocketHandler> UnsafeDisconnected;

        /// <summary>
        /// Client side only.
        /// </summary>
        public event EventHandler<SocketHandler> ClientToServerConnected;

        /// <summary>
        /// Server side only.
        /// </summary>
        public event EventHandler<SocketHandler> ServerToClientConnected;

        /// <summary>
        /// END OF TRANSMISSION
        /// </summary>
        public const byte EOT = 0x04;

        public SocketHandler(SocketHandlerMode mode, IPAddress ip, int port)
        {
            socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            switch (mode)
            {
                case SocketHandlerMode.Listen:
                    socket.Bind(new IPEndPoint(ip, port));
                    socket.Listen(port);
                    listenerTask = new Task(() => { while (!disposing) { Listen().WaitOne(); } }, TaskCreationOptions.LongRunning);
                    break;

                case SocketHandlerMode.Connect:

                    Connect(new IPEndPoint(ip, port));
                    receiveTask = new Task(() => { while (!disposing) { Receive().WaitOne(); } }, TaskCreationOptions.LongRunning);
                    break;
            }

            listenerTask?.Start();
            receiveTask?.Start();
        }

        public SocketHandler(Socket socket)
        {
            this.socket = socket;
            receiveTask = new Task(() => { while (!disposing) { Receive().WaitOne(); } }, TaskCreationOptions.LongRunning);
            receiveTask?.Start();
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.disposing = true;
                    ReceiveDone.Set();
                    ListenDone.Set();
                    receiveTask?.Wait();
                    listenerTask?.Wait();
                    socket.Dispose();
                    socket = null;
                }

                disposedValue = true;
            }
        }

        ~SocketHandler()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose

        #region Receive

        public ManualResetEvent Receive()
        {
            ReceiveDone.Reset();
            try
            {
                // Create the state object.
                StateObject state = new StateObject
                {
                    workSocket = socket,
                    networkHandler = this
                };

                // Begin receiving the data from the remote device.
                socket?.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out SocketError errorCode, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                ReceiveDone.Set();
                Console.WriteLine(e.ToString());
                UnsafeDisconnected?.Invoke(this, this);
            }

            return ReceiveDone;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the client socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.
            int bytesRead = socket?.EndReceive(ar, out SocketError errorCode) ?? 0;
            state.data.AddRange(state.buffer.Slice(0, bytesRead));

            if (bytesRead > 0 && !state.data.Contains(EOT))
            {
                // Get the rest of data.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out SocketError errorCode2, new AsyncCallback(ReceiveCallback), ar.AsyncState);
            }
            else
            {
                // All the data has arrived; put it in response.
                if (state.data.Count > 1)
                {
                    // Removes END OF TRANSMISSION byte.
                    state.data.RemoveAll(x => x == EOT);
                    var package = Package.Decode(state);
                    switch (package.Command)
                    {
                        case Command.None:
                            OnReceive?.Invoke(this, package);
                            break;

                        case Command.Close:
                            Disconnected?.Invoke(this, this);
                            break;

                        case Command.Message:
                            OnReceive?.Invoke(this, package);
                            break;

                        case Command.Command:
                            OnReceive?.Invoke(this, package);
                            break;
                    }
                }

                // Signal that all bytes have been received.
                ReceiveDone.Set();
            }
        }

        #endregion Receive

        #region Send

        public ManualResetEvent Send(Package package)
        {
            sendPackage = package;
            SendDone.Reset();
            // Adds END OF TRANSMISSION byte.
            byte[] data = package.Encode().Add(EOT);
            // Begin sending the data to the remote device.
            socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), socket);

            return SendDone;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);

                OnSend?.Invoke(this, sendPackage);
                // Signal that all bytes have been sent.
                SendDone.Set();
            }
            catch (Exception e)
            {
                SendDone.Set();
                Console.WriteLine(e.ToString());
                UnsafeDisconnected?.Invoke(this, this);
            }
        }

        #endregion Send

        #region Disconnect

        public ManualResetEvent Disconnect()
        {
            DisconnectDone.Reset();
            Send(new Package(Command.Close)).WaitOne();
            socket.Shutdown(SocketShutdown.Both);
            socket.BeginDisconnect(false, new AsyncCallback(DisconnectCallback), socket);
            return DisconnectDone;
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            // Complete the disconnect request.
            Socket client = (Socket)ar.AsyncState;
            client.EndDisconnect(ar);
            Disconnected?.Invoke(this, this);
            // Signal that the disconnect is complete.
            DisconnectDone.Set();
        }

        #endregion Disconnect

        #region Connect

        public ManualResetEvent Connect(IPEndPoint remoteEP)
        {
            try
            {
                // Connect to the remote endpoint.
                socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ConnectDone.Set();
            }

            return ConnectDone;
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                ClientToServerConnected?.Invoke(this, this);

                // Signal that the connection has been made.
                ConnectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ConnectDone.Set();
            }
        }

        #endregion Connect

        #region Listen

        public ManualResetEvent Listen()
        {
            ListenDone.Reset();

            // Start an asynchronous socket to listen for connections.
            socket.BeginAccept(new AsyncCallback(ListenCallback), socket);

            return ListenDone;
        }

        private void ListenCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            ListenDone.Set();

            // Get the socket that handles the client request.
            Socket client = socket?.EndAccept(ar);
            ServerToClientConnected?.Invoke(this, new SocketHandler(client));
        }

        #endregion Listen
    }
}