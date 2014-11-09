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
            : this(new InternalWebHost(virtualPath, physicalPath))
        {
        }

        internal WebHost(IWebHost webHost)
        {
            host = webHost;
        }

        public static WebHost Current { get; internal set; }

        public FiddlerWebClient CreateClient()
        {
            return new FiddlerWebClient(host.ProcessRequest);
        }

        public FiddlerWebRequest CreateRequest(Uri requestUrl)
        {
            return new FiddlerWebRequest(requestUrl, host.ProcessRequest);
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
