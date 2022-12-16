using Pickup.Net;
using Unity.Barracuda;
using Unity.Netcode;
using UnityEngine.Networking.Match;

namespace Pickup.Players
{
    public class NetworkPlayerBehavior : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                SpawnPlayerEventServerRpc(NetworkManager.Singleton.LocalClientId);
            }
        }

        [ServerRpc]
        private void SpawnPlayerEventServerRpc(ulong id)
        {
            ((ServerIO)Assist.netIO).onPlayerSpawn.Invoke(id);
            D.Log(id);
        }
    }
}