using System;
using Pickup.Configs;
using Pickup.Contents.Players;

namespace Pickup.Contents.Players
{
    [Serializable]
    public class Weapon : ItemStack
    {
        public Weapon(EquipC type) : base(type)
        {
            
        }
    }
}