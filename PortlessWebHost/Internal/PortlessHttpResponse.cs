using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PortlessWebHost.Internal
{
    internal sealed class PortlessHttpResponse : HttpResponseWrapper
    {
        private readonly NameValueCollection headers;

        public PortlessHttpResponse(HttpResponse response)
            : base(response)
        {
            headers = new PortlessHttpHeaders(response);
        }

        public override NameValueCollection Headers
        {
            get { return headers; }
        }
    }
}
