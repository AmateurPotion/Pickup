using System;
using Pickup.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Pickup.World
{
    [Serializable]
    public class Field
    {
        [Header("Data")]
        [SerializeField] internal SerializableDictionary<Vector2Int, TileObject> dataMap;

        public UnityEvent<Vector2Int, TileObject> OnTileChange = new (); 

        public Field()
        {
        }

        public void SyncTilemap(Tilemap tilemap)
        {
            
        }
        
        public bool TryGetTile(int x, int y, out TileObject tile) => dataMap.TryGetValue(new Vector2Int(x, y), out tile);

        public void SetTile(int x, int y, TileObject tile)
        {
            var position = new Vector2Int(x, y);
            OnTileChange.Invoke(position, tile);
            dataMap[position] = tile;
        }

        internal void JustSetTile(int x, int y, TileObject tile) => dataMap[new(x, y)] = tile; 

        public TileObject this[int x, int y]
        {
            get => dataMap[new Vector2Int(x, y)];
            set => SetTile(x, y, value);
        }
    }
}