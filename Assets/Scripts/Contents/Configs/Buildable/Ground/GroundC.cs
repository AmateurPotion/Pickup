using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pickup.Contents.Configs.Buildable.Ground
{
    [CreateAssetMenu(fileName = "new Ground Config", menuName = "Config/Ground/Default Ground", order = 0)]
    public class GroundC : ScriptableObject
    {
        public TileBase tile;
    }
}