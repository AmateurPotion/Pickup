using System;
using System.Collections.Generic;
using Pickup.Configs.Buildable.Structure;
using Pickup.Net;
using Pickup.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Pickup.Contents
{
    public sealed class ContentLoader : MonoBehaviour
    {
        public AssetLabelReference tileRef = new (){labelString = "Tiles"};
        public AssetLabelReference structureRef = new(){labelString = "Structure"};
        
        // Panels
        [SerializeField] private Canvas panelCanvas;
        [SerializeField] private Panel settingPanel;
        
        // Net
        public GameObject hostIOPrefab;
        public GameObject clientIOPrefab;

        private void Awake()
        {
            var loadTasks = new List<AsyncOperationHandle>();
            
            loadTasks.Add(Addressables.LoadAssetsAsync<RuleTile>(tileRef, config =>
            {
                Assist.contents.tiles[config.name] = config;
            }));
            
            loadTasks.Add(Addressables.LoadAssetsAsync<StructureC>(structureRef, config =>
            {
                Assist.contents.structures[config.name] = config;
            }));

            // finish load contents
            foreach (var task in loadTasks)
            {
                task.WaitForCompletion();
                switch (task.Result)
                {
                    case List<RuleTile> list:
                    {
                        Debug.Log($"Tiles {list.Count} loaded");
                        break;
                    }
                    case List<StructureC> list:
                    {
                        Debug.Log($"Structures {list.Count} loaded");
                        break;
                    }
                }
            }

            NetworkIO.clientIOPrefab = clientIOPrefab;
            NetworkIO.hostIOPrefab = hostIOPrefab;
            
            // loading panel
            DontDestroyOnLoad(panelCanvas);
            Assist.panelManager.setting = settingPanel;
        }
    }
}