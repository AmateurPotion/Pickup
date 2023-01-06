using System;
using System.Collections.Generic;
using Pickup.Net;
using Pickup.Graphics.UI;
using Pickup.Graphics.UI.Panels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Pickup.Contents
{
    public sealed class ContentLoader : MonoBehaviour
    {
        [Header("Addressable")]
        public AssetLabelReference tileRef = new (){labelString = "Tiles"};
        public AssetLabelReference structureRef = new(){labelString = "Structure"};

        [Header("NetworkObject")]
        public GameObject hostIOPrefab;
        public GameObject clientIOPrefab;

        [Header("Panel")]
        [SerializeField] private Canvas panelCanvas;
        [SerializeField] private Panel settingPanel;

        public void Load()
        {
            var loadTasks = new Dictionary<string, AsyncOperationHandle>()
            {
                ["Tiles"] = Addressables.LoadAssetsAsync<RuleTile>(tileRef, config =>
                {
                    Assist.contents.tiles[config.name] = config;
                }),
                ["Structures"] = Addressables.LoadAssetsAsync<GameObject>(structureRef, obj =>
                {
                    Assist.contents.structures[obj.name] = obj;
                })
            };

            // finish load contents
            foreach (var task in loadTasks)
            {
                task.Value.WaitForCompletion();

                var list = (List<object>)task.Value.Result;
                
                Debug.Log($"{task.Key} {list.Count} loaded");
            }

            NetworkIO.clientIOPrefab = clientIOPrefab;
            NetworkIO.hostIOPrefab = hostIOPrefab;

            // loading panel
            DontDestroyOnLoad(panelCanvas);

            var panelManager = Assist.panelManager;
            
            panelManager.canvas = panelCanvas;
            panelManager.setting = settingPanel;
            
            foreach (var panel in panelManager.panelDic)
            {
                panel.Value.gameObject.SetActive(false);
            }
        }
    }
}