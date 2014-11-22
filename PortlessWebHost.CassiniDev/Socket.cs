using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CassiniDev
{
    public class Socket : IDisposable
    {
        private static readonly Func<EventWaitHandle> defaultShutdownLockFactory = () => new ManualResetEvent(false);
        private static Func<EventWaitHandle> shutdownLockFactory;
        private readonly Stream inputStream;
        private readonly MemoryStream outputStream;
        private readonly EventWaitHandle shutdownLock;

        public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
        }

        public Socket(byte[] requestBytes)
        {
            inputStream = new MemoryStream(requestBytes);
            outputStream = new MemoryStream();
            shutdownLock = ShutdownLockFactory();
        }

        public static Func<EventWaitHandle> ShutdownLockFactory
        {
            get { return shutdownLockFactory ?? defaultShutdownLockFactory; }
            set { shutdownLockFactory = value; }
        }

        public int Available
        {
            get { return (int)(inputStream.Length - inputStream.Position); }
        }

        public bool Connected
        {
            get { return true; }
        }

        public EndPoint LocalEndPoint
        {
            get { return null; }
        }

        public EndPoint RemoteEndPoint
        {
            get { return null; }
        }

        public EventWaitHandle ShutdownLock
        {
            get { return shutdownLock; }
        }

        public bool Poll(int microSeconds, SelectMode mode)
        {
            return false;
        }

        public Socket Accept()
        {
            return this;
        }

        public void Bind(EndPoint localEndPoint)
        {
        }

        public void Close()
        {
        }

        public void Dispose()
        {
            inputStream.Dispose();
            outputStream.Dispose();
            shutdownLock.Dispose();
        }

        public void Listen(int backlog)
        {
        }

        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return inputStream.Read(buffer, offset, size);
        }

        public int Send(byte[] buffer)
        {
            outputStream.Write(buffer, 0, buffer.Length);
            return buffer.Length;
        }

        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            outputStream.Write(buffer, offset, size);
            return size;
        }

        public void Shutdown(SocketShutdown how)
        {
            shutdownLock.Set();
        }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
        {
        }

        public byte[] ToArray()
        {
            return outputStream.ToArray();
        }
    }
}
