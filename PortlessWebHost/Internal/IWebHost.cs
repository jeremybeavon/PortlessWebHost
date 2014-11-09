using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortlessWebHost.Internal
{
    internal interface IWebHost
    {
        byte[] ProcessRequest(byte[] requestBytes);
    }
}
