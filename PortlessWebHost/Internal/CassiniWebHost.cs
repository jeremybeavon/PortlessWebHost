using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using AppDomainCallbackExtensions;
using CassiniDev;

namespace PortlessWebHost.Internal
{
    internal sealed class CassiniWebHost : MarshalByRefObject, IRegisteredObject, IWebHost
    {
        private const int DummyPort = 12345;

        [ThreadStatic]
        private static bool waitForShutdown;

        private Host host;
        private Server server;
        private bool isSecure;

        public CassiniWebHost()
        {
            WebHost.Current = new WebHost(this);
        }

        public AppDomain Domain
        {
            get { return AppDomain.CurrentDomain; }
        }

        public static void WaitForShutdown()
        {
            waitForShutdown = true;
        }

        public void Configure(string virtualPath, string physicalPath, bool isSecure)
        {
            server = new Server(DummyPort, virtualPath, physicalPath);
            host = new Host();
            host.Configure(server, DummyPort, virtualPath, physicalPath);
            AppDomain.CurrentDomain.AddResolveDirectory(Path.GetDirectoryName(typeof(PortlessModule).Assembly.Location));
            HttpApplication.RegisterModule(typeof(PortlessModule));
            this.isSecure = isSecure;
        }

        public byte[] ProcessRequest(byte[] requestBytes)
        {
            using (var socket = new Socket(requestBytes))
            {
                new PortlessRequest(server, host, socket, isSecure).Process();
                if (waitForShutdown)
                {
                    socket.ShutdownLock.WaitOne();
                    waitForShutdown = false;
                }

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
