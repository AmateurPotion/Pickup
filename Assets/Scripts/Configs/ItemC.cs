using System;
using UnityEngine;

namespace Pickup.Configs
{
    [CreateAssetMenu(fileName = "new Item Config", menuName = "Config/Item/Default Item", order = 0), Serializable]
    public class ItemC : ScriptableObject
    {
        public bool stackable;
        public int maxStack;
    }
}