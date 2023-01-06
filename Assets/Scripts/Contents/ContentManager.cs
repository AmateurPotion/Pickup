using System.Collections.Generic;
using UnityEngine;

namespace Pickup.Contents
{
    public sealed class ContentManager
    {
        public readonly Dictionary<string, RuleTile> tiles;
        public readonly Dictionary<string, GameObject> structures;
        
        public ContentManager()
        {
            tiles = new Dictionary<string, RuleTile>();
            structures = new Dictionary<string, GameObject>();
        }
    }
}