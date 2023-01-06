using Pickup.Contents.Items;
using Pickup.Net;
using Pickup.Scenes.LobbyScene;
using Pickup.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Pickup.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent<Vector2> onMove = new();
        
        [Header("Components")]
        [SerializeField] private Rigidbody2D body;
        public LineRenderer aim;

        [Header("Network")] 
        public NetworkPlayerBehavior netPlayer;
        
        [Header("Stats")] 
        public GaugeInt health = new(0, 1000, 1000);
        public GaugeInt energy = new(0, 1000, 1000);
        public int speed = 35;
        
        [Header("Inventory")]
        public SerializableDictionary<int, ItemStack> items;
        public Weapon weapon;

        public void Init(bool multiPlay = false)
        {
            if (!multiPlay)
            {
                // single
                onMove.AddListener(direction =>
                {
                    body.AddForce(direction * speed);
                });
            }
            else
            {
                // multiPlay
                onMove.AddListener(direction =>
                {
                    if (!NetworkManager.Singleton.IsHost)
                    {
                        ClientIO.localClientIO.MovePlayerServerRpc(direction);              
                    }
                });
            }
        }

        private Collision2D beforeWall;
        public void OnCollisionEnter2D(Collision2D col)
        {
        }
    }
}