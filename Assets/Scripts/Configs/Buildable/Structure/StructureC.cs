using Pickup.Scenes.FieldScene;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pickup.Configs.Buildable.Structure
{
    [CreateAssetMenu(fileName = "new Structure Config", menuName = "Config/Structure/Default Structure", order = 0)]
    public class StructureC : ScriptableObject
    {
        public RuleTile sprite;
        public bool update = false;
        
        public virtual void UpdateO(StructureM obj)
        {
            
        }
        
        public virtual void Check(StructureM obj)
        {
            
        }
    }
}