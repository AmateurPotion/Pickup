using System;
using System.IO;
using Pickup.Contents;
using Pickup.Net;
using Pickup.UI.Panels;
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