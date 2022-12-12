using System.Collections.Generic;
using UnityEngine.Events;

namespace Pickup.Net
{
    public class ServerIO : NetworkIO
    {
        public Dictionary<ulong, ClientIO> clients = new();
        public UnityEvent<ulong> onPlayerSpawn;
    }
}