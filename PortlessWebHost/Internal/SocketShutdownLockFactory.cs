using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CassiniDev;

namespace PortlessWebHost.Internal
{
    public static class SocketShutdownLockFactory
    {
        public static void Override(Func<EventWaitHandle> shutdownLockFactory)
        {
            Socket.ShutdownLockFactory = shutdownLockFactory;
        }
    }
}
