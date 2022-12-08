using Pickup.Scenes.FieldScene;
using UnityEngine;

namespace Pickup.Configs.Buildable.Structure
{
    [CreateAssetMenu(fileName = "new Anchor Config", menuName = "Config/Structure/Anchor", order = 0)]

    public class AnchorC : StructureC
    {
        public int range;

        public override void Check(StructureM obj)
        {
            var data = obj.tags;
        }

        public override void UpdateO(StructureM obj)
        {
        }
    }
}