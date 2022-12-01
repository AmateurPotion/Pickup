using System;
using Pickup.Configs;
using Pickup.Contents.Items.Equipments;
using Pickup.Utils;
using UnityEngine;

namespace Pickup.Contents.Players
{
    [Serializable]
    public class ItemStack
    {
        public ItemC type;
        public GaugeInt amount;
        
        public ItemStack(ItemC type)
        {
            this.type = type;
        }

        public bool IsWeapon(out Weapon stack)
        {
            var equal = type is EquipC;
            stack = equal ? (Weapon)this : null;

            return equal;
        }
    }
}