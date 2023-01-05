using System;
using System.IO;
using Pickup.Contents;
using Pickup.FileSystem;
using Pickup.Net;
using Pickup.Graphics.UI.Panels;
using UnityEngine;

namespace Pickup
{
    public static class Assist
    {
        // States
        public static string uuid;
        public static string dataPath => Path.Combine(Application.dataPath, "Data");
        
        // Managers
        public static readonly ContentManager contents = new();
        public static readonly MainPanelManager panelManager = new();
        public static readonly FSManager fs = new();

        // Network
        public static NetworkIO netIO;

        internal static void Initialize()
        {
            fs.Init();
            uuid = PlayerPrefs.GetString("uuid", Guid.NewGuid().ToString());
            //PlayerPrefs.SetString("uuid", uuid);
            //PlayerPrefs.Save();
        }
    }
}