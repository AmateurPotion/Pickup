using System.Collections.Generic;
using Pickup.Contents.Configs.Buildable.Structure;
using UnityEngine;

namespace Pickup.Contents
{
    public sealed class ContentManager
    {
        public readonly Dictionary<string, RuleTile> tiles;
        public readonly Dictionary<string, StructureC> structures;
        
        public ContentManager()
        {
            tiles = new Dictionary<string, RuleTile>();
            structures = new Dictionary<string, StructureC>();
        }
    }
}