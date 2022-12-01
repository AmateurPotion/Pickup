using System;
using Pickup.Utils.Attributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pickup.World
{
    [Serializable]
    public class TileObject
    {
        [SerializeField, GetSet("ground")] private TileBase _ground;
        public TileBase ground;

        public TileObject()
        {
        }

    }
}