using System;
using Pickup.Configs.Buildable.Structure;
using Pickup.Utils.Tags;
using UnityEngine;

namespace Pickup.Scenes.FieldScene
{
    public partial class StructureM : MonoBehaviour
    {
        private StructureC m_Type;

        public StructureC type
        {
            get => m_Type;
            set
            {
                value.Check(this);
                m_Type = value;
            }
        }

        public readonly TagCompound tags = new ()
        {
            ["health"] = 100
        };

        private void Update()
        {
            if (m_Type && type.update) type.UpdateO(this);
        }
    }
}