using Pickup.Utils.Tags;
using SRF;
using UnityEngine;

namespace Pickup.Scenes.FieldScene
{
    public partial class StructureM : MonoBehaviour
    {
        public readonly TagCompound tags = new ()
        {
            ["health"] = 100
        };

        public void Release()
        {
            transform.DestroyChildren();
        }
    }
}