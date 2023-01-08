using System.Collections.Generic;
using Pickup.Scenes.FieldScene;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Pickup.Contents.Configs.Structures
{
    public class StructureC <T> : ScriptableObject where T : StructureC<T>
    {
        private static readonly Dictionary<string, T> _instances = new();
        
        public static T GetInstance(string id)
        {
            if (_instances.TryGetValue(id, out var instance))
            {
                return instance;
            }
            else
            {
                var handle = Addressables.LoadAssetAsync<T>(id);

                handle.Completed += operationHandle =>
                {
                    if (operationHandle.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.Log($"Structure loading is failed. reason: cannot find objectId {id}");
                    }
                };
                
                return _instances[id] = handle.WaitForCompletion();
            }
        }

        public virtual void Alloc(StructureM obj)
        {
        }
    }
}