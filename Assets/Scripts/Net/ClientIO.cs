using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pickup.Players;
using Unity.Netcode;
using UnityEngine;

namespace Pickup.Net
{
    public class ClientIO : NetworkIO
    {
        private static NetworkManager manager => NetworkManager.Singleton;
        public static ClientIO localClientIO;

        public Player player;
        
        [ClientRpc]
        public void InitClientIOClientRpc(ulong id)
        {
            if (manager.IsHost)
            {
                player = manager.ConnectedClients[id].PlayerObject.GetComponent<Player>();
            }
            else
            {
                player = manager.LocalClient.PlayerObject.GetComponent<Player>();

                if (id == manager.LocalClientId)
                {
                    localClientIO = this;
                    PlayerManager.Instance.InitPlayer(player);
                }
            }
        }
        
        // PlayerMethod
        [ServerRpc]
        public void MovePlayerServerRpc(Vector2 direction)
        {
            if (manager.IsServer)
            {
                player.onMove.Invoke(direction);
            }
        }
    }
}