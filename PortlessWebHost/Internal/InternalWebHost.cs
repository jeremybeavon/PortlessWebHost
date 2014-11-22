using System;
using System.IO;
using System.Web.Hosting;
using AppDomainCallbackExtensions;

namespace PortlessWebHost.Internal
{
    internal sealed class InternalWebHost : IDisposable, IWebHost
    {
        private readonly string appId;
        private readonly ApplicationManager applicationManager;

        public InternalWebHost(string virtualPath, string physicalPath, Protocol protocol)
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

            Host = Domain.CreateInstanceFromAndUnwrap<CassiniWebHost>();
            HostingEnvironment.RegisterObject(Host);
            HostingEnvironment.UnregisterObject(defaultHost);
            Host.Configure(virtualPath, physicalPath, protocol == Protocol.Https);
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
