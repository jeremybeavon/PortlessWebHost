using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace PortlessWebHost.Internal
{
    internal sealed class PortlessHttpHeaders : NameValueCollection
    {
        private readonly HttpResponse response;

        public PortlessHttpHeaders(HttpResponse response)
        {
            this.response = response;
        }

        public override void Add(string name, string value)
        {
            base.Add(name, value);
            response.AppendHeader(name, value);
        }

        public override void Set(string name, string value)
        {
            base.Set(name, value);
            response.AppendHeader(name, value);
        }
    }
}
