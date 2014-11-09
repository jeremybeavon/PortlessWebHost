using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PortlessWebHost
{
    public sealed class FiddlerWebClient : WebClient
    {
        private readonly Func<byte[], byte[]> processRequestFunc;

        internal FiddlerWebClient(Func<byte[], byte[]> processRequestFunc)
        {
            this.processRequestFunc = processRequestFunc;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            return new FiddlerWebRequest(address, processRequestFunc);
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            return ((FiddlerWebRequest)request).GetResponse();
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            return GetWebResponse(request);
        }
    }
}
