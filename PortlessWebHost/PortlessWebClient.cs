using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PortlessWebHost
{
    public sealed class PortlessWebClient : WebClient
    {
        private readonly Func<byte[], byte[]> processRequestFunc;

        internal PortlessWebClient(Func<byte[], byte[]> processRequestFunc)
        {
            this.processRequestFunc = processRequestFunc;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            return new PortlessWebRequest(address, processRequestFunc);
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            return ((PortlessWebRequest)request).GetResponse();
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            return GetWebResponse(request);
        }
    }
}
