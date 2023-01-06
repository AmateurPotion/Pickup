using System;
using Pickup.Utils.Tags;
using UnityEngine;

namespace Pickup.Scenes.FieldScene
{
    public partial class StructureM : MonoBehaviour
    {
        public readonly TagCompound tags = new ()
        {
            ["health"] = 100
        };

    }
}