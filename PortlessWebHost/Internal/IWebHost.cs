using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortlessWebHost.Internal
{
    internal interface IWebHost
    {
        AppDomain Domain { get; }

        byte[] ProcessRequest(byte[] requestBytes);
    }
}
