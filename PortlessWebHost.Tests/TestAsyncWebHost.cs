using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortlessWebHost.Internal;

namespace PortlessWebHost.Tests
{
    public sealed class TestAsyncWebHost : IDisposable
    {
        private readonly WebHost host;

        public TestAsyncWebHost(string virtualPath, string physicalPath)
        {
            host = new WebHost(virtualPath, physicalPath);
            host.Domain.DoCallBack(SetUpAsyncCall);
        }

        public PortlessWebRequest CreateRequest(Uri requestUrl)
        {
            return host.CreateRequest(requestUrl);
        }

        public void Dispose()
        {
            host.Dispose();
        }

        private static void SetUpAsyncCall()
        {
            SocketShutdownLockFactory.Override(() => new TestEventWaitHandle());
        }
    }
}
