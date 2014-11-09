using System;
using System.IO;
using System.Web.Hosting;

namespace PortlessWebHost.Internal
{
    internal sealed class InternalWebHost : IDisposable, IWebHost
    {
        private readonly string appId;
        private readonly ApplicationManager applicationManager;

        public InternalWebHost(string virtualPath, string physicalPath)
        {
            appId = (virtualPath + physicalPath).GetHashCode().ToString("x");
            applicationManager = ApplicationManager.GetApplicationManager();
            IRegisteredObject defaultHost = applicationManager.CreateObject(
                appId,
                typeof(ISAPIRuntime),
                virtualPath,
                physicalPath,
                false,
                true);
            Domain = applicationManager.GetAppDomain(appId);
            if (defaultHost == null || Domain == null)
                throw new NotSupportedException();

            Host = (CassiniWebHost)Domain.CreateInstanceFromAndUnwrap(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(typeof(CassiniWebHost).Assembly.Location)),
                typeof(CassiniWebHost).FullName);
            HostingEnvironment.RegisterObject(Host);
            HostingEnvironment.UnregisterObject(defaultHost);
            Host.Configure(virtualPath, physicalPath);
        }

        public CassiniWebHost Host { get; private set; }

        public AppDomain Domain { get; private set; }

        public void Dispose()
        {
            applicationManager.ShutdownApplication(appId);
        }

        public byte[] ProcessRequest(byte[] requestBytes)
        {
            return Host.ProcessRequest(requestBytes);
        }
    }
}
