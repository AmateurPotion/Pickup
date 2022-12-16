using System;
using System.Collections.Generic;
using System.IO;
using Pickup.Configs.Buildable.Structure;
using Pickup.Contents;
using Pickup.Net;
using Pickup.UI;
using Pickup.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

namespace Pickup
{
    public static class Assist
    {
        // States
        public static string uuid;
        public static string dataPath => Path.Combine(Application.dataPath, "Data");
        
        // Managers
        public static readonly ContentManager contents = new();
        public static readonly PanelManager panelManager = new();

        // Network
        public static NetworkIO netIO;

        private static void Initialize()
        {
            uuid = PlayerPrefs.GetString("uuid", Guid.NewGuid().ToString());
            //PlayerPrefs.SetString("uuid", uuid);
            //PlayerPrefs.Save();
        }
    }
}