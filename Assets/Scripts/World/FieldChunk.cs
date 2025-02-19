﻿using System;
using System.Collections.Generic;
using System.Linq;
using Pickup.Utils.Tags;
using UnityEngine;

namespace Pickup.World
{
    public class FieldChunk
    {
        public const byte Size = 100;

        internal int[] tileMapData;
        internal int[] structureIDData;
        internal TagCompound[] structureData;
        
        public FieldChunk(int baseTileId)
        {
            tileMapData = Enumerable.Repeat(baseTileId, Size * Size).ToArray();
            structureIDData = Enumerable.Repeat(0, Size * Size).ToArray();
            structureData = Enumerable.Repeat<TagCompound>(null, Size * Size).ToArray();
        }

        public FieldChunk(RuleTile baseTile) : this(baseTile.name.GetHashCode())
        {
        }
    }
}