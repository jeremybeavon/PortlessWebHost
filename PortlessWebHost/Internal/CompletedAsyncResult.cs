using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PortlessWebHost.Internal
{
    internal sealed class CompletedAsyncResult : IAsyncResult
    {
        public CompletedAsyncResult(AsyncCallback callback, object state)
        {
            AsyncState = state;
            if (callback != null)
            {
                callback(this);
            }
        }

        public object AsyncState { get; private set; }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotSupportedException(); }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }

        public bool IsCompleted
        {
            get { return true; }
        }
    }
}
