using UnityEngine;

namespace Pickup.Scenes.FieldScene
{
    public partial class StructureM
    {
        public int health
        {
            get => tags.GetInt("health");
            set => tags["health"] = value;
        }
    }
}