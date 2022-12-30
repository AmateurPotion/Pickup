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
        [Header("Addressable")]
        public AssetLabelReference tileRef = new (){labelString = "Tiles"};
        public AssetLabelReference structureRef = new(){labelString = "Structure"};

        [Header("NetworkObject")]
        public GameObject hostIOPrefab;
        public GameObject clientIOPrefab;

        [Header("Panel")]
        [SerializeField] private Canvas panelCanvas;
        [SerializeField] private Panel settingPanel;

        private void Awake()
        {
            var loadTasks = new List<AsyncOperationHandle>
            {
                Addressables.LoadAssetsAsync<RuleTile>(tileRef, config =>
                {
                    Assist.contents.tiles[config.name] = config;
                }),
                Addressables.LoadAssetsAsync<StructureC>(structureRef, config =>
                {
                    Assist.contents.structures[config.name] = config;
                })
            };

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