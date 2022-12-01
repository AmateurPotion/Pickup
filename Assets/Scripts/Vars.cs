using System;
using System.Collections.Generic;
using System.IO;
using Pickup.Configs.Buildable;
using Pickup.Configs.Buildable.Ground;
using Pickup.Configs.Buildable.Structure;
using Pickup.Contents;
using Pickup.Net;
using Pickup.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

namespace Pickup
{
    public sealed partial class Vars : MonoBehaviour
    {
        public static Vars Instance { get; private set; }
        [Header("States")]
        public string uuid;
        public string dataPath => Path.Combine(Application.dataPath, "Data");
        
        [Header("Managers")]
        public ContentManager contents = ContentManager.instance;

        [Header("Types")] 
        public TileBase nullTileBase;
        public AssetLabelReference groundRef = new (){labelString = "Ground"};
        public SerializableDictionary<string, GroundC> grounds = new();
        public AssetLabelReference structureRef = new(){labelString = "Structure"};
        public SerializableDictionary<string, StructureC> structures = new();

        [Header("Network")] 
        // ServerIO or ClientIO
        public NetworkIO netIO;
        public NetworkManager networkManager;

        [Header("Network Prefab")] 
        [SerializeField] private List<GameObject> _networkPrefabs;

        public SerializableDictionary<string, GameObject> networkPrefabs;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Check data space
            //if (!Directory.Exists(dataPath) && Application.platform == editor) Directory.CreateDirectory(dataPath);
            // load && alloc contents
            var loadTasks = new List<AsyncOperationHandle>();

            loadTasks.Add(Addressables.LoadAssetsAsync<GroundC>(groundRef, config =>
            {
                grounds[config.name] = config;
            }));
            
            loadTasks.Add(Addressables.LoadAssetsAsync<StructureC>(structureRef, config =>
            {
                structures[config.name] = config;
            }));

            // finish load contents
            foreach (var task in loadTasks)
            {
                task.WaitForCompletion();
                switch (task.Result)
                {
                    case List<GroundC> list:
                    {
                        Debug.Log($"Grounds {list.Count} loaded");
                        break;
                    }
                    case List<StructureC> list:
                    {
                        Debug.Log($"Structures {list.Count} loaded");
                        break;
                    }
                }
            }
            
            uuid = PlayerPrefs.GetString("uuid", Guid.NewGuid().ToString());
            //PlayerPrefs.SetString("uuid", uuid);
            //PlayerPrefs.Save();
            
            DontDestroyOnLoad(this);
            Instance = this;
        }

        private void Start()
        {
            // Network init
            networkManager.SetSingleton();
            
            foreach (var networkObject in _networkPrefabs)
            {
                networkManager.AddNetworkPrefab(networkObject);
                networkPrefabs[networkObject.name] = networkObject;
            }
            _networkPrefabs.Clear();
            _networkPrefabs = null;
        }
    }
}