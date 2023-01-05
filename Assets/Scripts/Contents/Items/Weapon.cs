using System;
using Pickup.Contents.Configs;
using Pickup.Contents.Items;

namespace Pickup.Contents.Items
{
    [Serializable]
    public class Weapon : ItemStack
    {
        public Weapon(EquipC type) : base(type)
        {
            
        }
    }
}