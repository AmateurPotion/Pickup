using System;
using Pickup.Players;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Pickup.Net
{
    public abstract class NetworkIO : NetworkBehaviour
    {
        public static State state = State.UnActive;
        private static NetworkManager manager => NetworkManager.Singleton;
        public static ulong netId => manager.LocalClientId;

        internal static GameObject hostIOPrefab;
        internal static GameObject clientIOPrefab;

        public const ushort DefaultPort = 7777;

        public static bool Connect(string address) => Connect(address, DefaultPort);
        protected static bool Connect(string address, ushort port)
        {
            if (state != State.UnActive) return false;
            
            // configure transport
            if (manager.NetworkConfig.NetworkTransport is UnityTransport transport)
            {
                transport.ConnectionData = new()
                {
                    Address = address,
                    Port = port
                };
            }

            // start Connect
            if (!manager.StartClient()) return false;
            state = State.Client;

            return true;
        }

        public static bool Host(out HostIO io) => Host(DefaultPort, out io);
        protected static bool Host(ushort port, out HostIO io)
        {
            io = null;
            if (state != State.UnActive) return false;
            
            // configure transport
            if (manager.NetworkConfig.NetworkTransport is UnityTransport transport)
            {
                transport.ConnectionData = new()
                {
                    Address = "127.0.0.1",
                    Port = port,
                    ServerListenAddress = "127.0.0.1"
                };
            }

            // Host server
            if (!manager.StartHost()) return false;
            state = State.Host;

            while (!manager.IsListening) { }
            
            var hostObj = Instantiate(hostIOPrefab);
            Assist.netIO = hostObj.GetComponent<HostIO>();
            hostObj.name = "HostIO-Instance";
            hostObj.GetComponent<NetworkObject>().Spawn();
            
            manager.OnClientConnectedCallback += RegisterClient;
            manager.OnClientDisconnectCallback += DisConnectClient;

            PlayerManager.Instance.InitPlayer(manager.LocalClient.PlayerObject.GetComponent<Player>());
            
            return true;
        }

        private static void RegisterClient(ulong clientId)
        {
            var g = Instantiate(clientIOPrefab);
            g.name = $"ClientIO-{clientId}";
            var clientIO = ((ServerIO)Assist.netIO).clients[clientId] = g.GetComponent<ClientIO>();
            g.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            var playerObj = manager.ConnectedClients[clientId].PlayerObject;
            playerObj.ChangeOwnership(clientId);
            
            clientIO.InitClientIOClientRpc(clientId);
        }

        private static void DisConnectClient(ulong clientId)
        {
            ((ServerIO)Assist.netIO).clients.Remove(clientId);
        }

        public override void OnDestroy()
        {
            //manager.OnClientConnectedCallback -= RegisterClient;
            state = State.UnActive;
            base.OnDestroy();
        }
        
        // rpc
        
        public event Action<string> onMessageReceive;
        
        private void Awake()
        {
            onMessageReceive += Debug.Log;
        }

        [ClientRpc]
        public void SendMessageClientRpc(string data, ulong id)
        {
            if (id == NetworkManager.LocalClientId)
            {
                onMessageReceive?.Invoke(data);
            }
        }
        
        [ClientRpc]
        public void SendMessageClientRpc(string data)
        {
            Debug.Log(data);
            //onMessageReceive?.Invoke(data);
        }

        public enum State
        {
            Host, Server, Client, UnActive
        }
    }
}