using Pickup.Contents.Items;
using Pickup.Net;
using Pickup.Utils;
using Unity.Netcode;
using UnityEngine;

namespace Pickup.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
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

        public void Move(Vector2 direction)
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                ClientIO.localClientIO.MovePlayerServerRpc(direction);              
            }
            else
            {
                body.AddForce(direction * speed);
            }
        }

        private Collision2D beforeWall;
        public void OnCollisionEnter2D(Collision2D col)
        {
        }
    }
}