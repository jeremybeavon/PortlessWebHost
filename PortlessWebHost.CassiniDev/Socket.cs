using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CassiniDev
{
    public class Socket : IDisposable
    {
        private readonly Stream inputStream;
        private readonly MemoryStream outputStream;

        public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
        }

        public Socket(byte[] requestBytes)
        {
            inputStream = new MemoryStream(requestBytes);
            outputStream = new MemoryStream();
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
