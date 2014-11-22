using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortlessWebHost.Internal;

namespace PortlessWebHost
{
    public sealed class WebHost : IDisposable
    {
        private readonly IWebHost host;

        public WebHost(string virtualPath, string physicalPath)
            : this(virtualPath, physicalPath, Protocol.Http)
        {
        }

        public WebHost(string virtualPath, string physicalPath, Protocol protocol)
            : this(new InternalWebHost(virtualPath, physicalPath, protocol))
        {
        }

        internal WebHost(IWebHost webHost)
        {
            host = webHost;
        }

        public static WebHost Current { get; internal set; }

        public AppDomain Domain
        {
            get { return host.Domain; }
        }

        public PortlessWebClient CreateClient()
        {
            return new PortlessWebClient(host.ProcessRequest);
        }

        public PortlessWebRequest CreateRequest(Uri requestUrl)
        {
            return new PortlessWebRequest(requestUrl, host.ProcessRequest);
        }

        public void Dispose()
        {
            IDisposable disposable = host as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
