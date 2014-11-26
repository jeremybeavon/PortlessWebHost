using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PortlessWebHost.TestWebSite
{
    /// <summary>
    /// Summary description for AsyncRequest
    /// </summary>
    public class AsyncRequest : IHttpAsyncHandler, IAsyncResult
    {
        private AsyncCallback callback;
        private HttpContext context;

        public AsyncRequest()
        {
            Current = this;
        }

        public static AsyncRequest Current { get; private set; }

        public object AsyncState { get; private set; }

        public WaitHandle AsyncWaitHandle
        {
            get { return null; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted { get; set; }

        public bool IsReusable
        {
            get { return false; }
        }

        public void FinishRequest()
        {
            context.Request.RequestContext.HttpContext.Response.Headers.Add("Set-Cookie", "Cookie1=test1");
            context.Request.RequestContext.HttpContext.Response.Headers.Add("Set-Cookie", "Cookie2=test2");
            context.Response.Write("Async test succeeded");
            IsCompleted = true;
            callback(this);
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotSupportedException();
        }
        
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            this.context = context;
            callback = cb;
            AsyncState = extraData;
            return this;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
        }
    }
}