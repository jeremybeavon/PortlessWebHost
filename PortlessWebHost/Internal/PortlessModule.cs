using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace PortlessWebHost.Internal
{
    internal sealed class PortlessModule : IHttpModule
    {
        private static readonly Guid itemKey = Guid.NewGuid();

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.PostMapRequestHandler += PostMapRequest;
        }

        public void Dispose()
        {
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            PortlessHttpContext portlessContext = new PortlessHttpContext(context);
            context.Request.RequestContext.HttpContext = portlessContext;
            context.Items[itemKey] = portlessContext;
            CassiniWebHost.WaitForShutdown();
        }

        private void PostMapRequest(object sender, EventArgs e)
        {
            RequestContext requestContext = HttpContext.Current.Request.RequestContext;
            if (!(requestContext.HttpContext is PortlessHttpContext))
            {
                requestContext.HttpContext = (HttpContextBase)HttpContext.Current.Items[itemKey];
            }
        }
    }
}
