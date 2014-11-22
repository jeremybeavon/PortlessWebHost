using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PortlessWebHost.TestWebSite;

namespace PortlessWebHost.Tests
{
    internal sealed class TestEventWaitHandle : EventWaitHandle
    {
        public TestEventWaitHandle()
            : base(false, EventResetMode.ManualReset)
        {
        }

        public override bool WaitOne()
        {
            AsyncRequest.Current.FinishRequest();
            return base.WaitOne();
        }
    }
}
