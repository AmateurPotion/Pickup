using System.Collections.Generic;
using Pickup.World;
using Pickup.World.Structures;
using Unity.Collections;
using UnityEngine;

namespace Pickup.Scenes.FieldScene
{
    public partial class StructureM : MonoBehaviour
    {
        private Structure _type;

        public Structure type
        {
            get => _type;
            set
            {
                value.Check(this);
                _type = value;
            }
        }

        public readonly Dictionary<string, dynamic> statCollection = new ()
        {
            ["health"] = 100
        };

        private void Update()
        {
            if (type.update) type.Update(this);
        }
    }
}