using System;
using UnityEngine;

namespace Pickup.Contents.Configs
{
    [CreateAssetMenu(fileName = "new Weapon Config", menuName = "Config/Item/Equipment/Default Weapon", order = 0)]
    public class WeaponC : EquipC
    {
        public WeaponFlag flag;
        public Sprite sprite;
    }

    [Flags]
    public enum WeaponFlag
    {
        Slash,
        Projectile
    }
}