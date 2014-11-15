using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PortlessWebHost.Internal
{
    internal sealed class PortlessHttpContext : HttpContextWrapper
    {
        private readonly HttpContext context;

        public PortlessHttpContext(HttpContext context)
            : base(context)
        {
            this.context = context;
        }

        public override HttpResponseBase Response
        {
            get { return new PortlessHttpResponse(context.Response); }
        }
    }
}
