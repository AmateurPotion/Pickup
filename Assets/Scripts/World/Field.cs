using System;
using System.Collections.Generic;
using Pickup.Contents.Configs.Structures;
using Pickup.Scenes.FieldScene;
using UnityEngine;
using UnityEngine.Pool;

namespace Pickup.World
{
    public class Field
    {
        protected readonly ObjectPool<StructureM> _pool;
        
        public Field(ObjectPool<StructureM> pool)
        {
            _pool = pool;
        }

        public StructureM SpawnStructure<T>(string id, Vector2Int position) where T : StructureC<T>
        {
            var config= StructureC<T>.GetInstance(id);
            var result = _pool.Get();
            result.name = $"{nameof(config)}_{id}";
            result.transform.position = new Vector3(position.x, position.y);
            config.Alloc(result);
            
            return result;
        }
    }
}