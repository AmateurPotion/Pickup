using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pickup.Configs.Buildable.Structure
{
    [CreateAssetMenu(fileName = "new Structure Config", menuName = "Config/Structure/Default Structure", order = 0)]
    public class StructureC : ScriptableObject
    {
        public RuleTile tile;
        public bool hasGameobject = false;
    }
}