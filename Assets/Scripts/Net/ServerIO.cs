using System.Collections.Generic;

namespace Pickup.Net
{
    public class ServerIO : NetworkIO
    {
        public Dictionary<ulong, ClientIO> clients = new();
    }
}