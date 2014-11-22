using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CassiniDev;

namespace PortlessWebHost.Internal
{
    internal sealed class PortlessRequest : Request
    {
        private readonly bool isSecure;

        public PortlessRequest(Server server, Host host, Socket socket, bool isSecure)
            : base(server, host, new Connection(server, socket))
        {
            this.isSecure = isSecure;
        }

        public override bool IsSecure()
        {
            return isSecure;
        }
    }
}
