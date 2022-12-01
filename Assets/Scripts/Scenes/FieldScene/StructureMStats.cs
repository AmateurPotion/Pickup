using UnityEngine;

namespace Pickup.Scenes.FieldScene
{
    public partial class StructureM
    {
        public int health
        {
            get => statCollection["health"];
            set => statCollection["health"] = value;
        }
    }
}