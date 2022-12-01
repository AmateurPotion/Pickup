using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;
using Pickup.Scenes.FieldScene;

namespace Pickup.World
{
    [Serializable]
    public class Structure
    {
        public int id => name.GetHashCode();
        public string name;
        public bool update = false;
        
        public Structure([NotNull] string name)
        {
            this.name = name;
        }

        public virtual void Create()
        {
            
        }

        public virtual void Update(StructureM obj)
        {
            
        }

        public virtual void Check(StructureM obj)
        {
            
        }
    }
}