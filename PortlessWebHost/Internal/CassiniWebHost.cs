using System;
using System.Web.Hosting;
using CassiniDev;

namespace PortlessWebHost.Internal
{
    internal sealed class CassiniWebHost : MarshalByRefObject, IRegisteredObject, IWebHost
    {
        private const int DummyPort = 12345;
        private Host host;
        private Server server;

        public CassiniWebHost()
        {
            WebHost.Current = new WebHost(this);
        }

        public void Configure(string virtualPath, string physicalPath)
        {
            server = new Server(DummyPort, virtualPath, physicalPath);
            host = new Host();
            host.Configure(server, DummyPort, virtualPath, physicalPath);
        }

        public byte[] ProcessRequest(byte[] requestBytes)
        {
            using (var socket = new Socket(requestBytes))
            {
                new Request(server, host, new Connection(server, socket)).Process();
                return socket.ToArray();
            }
        }

        public void Stop(bool immediate)
        {
            ((IRegisteredObject)host).Stop(immediate);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
