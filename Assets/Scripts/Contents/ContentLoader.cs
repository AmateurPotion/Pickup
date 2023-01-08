using System;
using System.Collections;
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
        [Header("NetworkObject")]
        public GameObject hostIOPrefab;
        public GameObject clientIOPrefab;

        [Header("Panel")]
        [SerializeField] private Canvas panelCanvas;
        [SerializeField] private Panel settingPanel;

        public void Load()
        {
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