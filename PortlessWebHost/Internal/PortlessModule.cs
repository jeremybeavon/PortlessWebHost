using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PortlessWebHost.Internal
{
    internal sealed class PortlessModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        public void Dispose()
        {
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            context.Request.RequestContext.HttpContext = new PortlessHttpContext(context);
            CassiniWebHost.WaitForShutdown();
        }
    }
}
