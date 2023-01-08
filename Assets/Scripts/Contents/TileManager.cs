using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Pickup.Contents
{
    public class TileManager
    {
        private readonly Dictionary<string, RuleTile> tiledata = new();

        public RuleTile this[string name]
        {
            get
            {
                if (tiledata.TryGetValue(name, out var tile))
                {
                    return tile;
                }
                else
                {
                    var handle = Addressables.LoadAssetAsync<RuleTile>($"Tiles{name}");

                    handle.Completed += operationHandle =>
                    {
                        if (operationHandle.Status == AsyncOperationStatus.Failed)
                        {
                            Debug.Log($"RuleTile loading is failed. reason: cannot find objectId {name}");
                        }
                    };
                
                    return tiledata[name] = handle.WaitForCompletion();
                }
            }
        }
    }
}