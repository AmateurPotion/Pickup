using UnityEngine;

namespace Pickup.Contents.Configs
{
    [CreateAssetMenu(fileName = "new Equipment Config", menuName = "Config/Item/Equipment/Default Equipment", order = 0)]
    public class EquipC : ItemC
    {
        public bool durable;
        public int maxDurability;
        public int startDurabilityDamage = 0;
    }
}