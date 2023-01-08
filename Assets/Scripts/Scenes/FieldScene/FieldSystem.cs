using System;
using System.Collections.Generic;
using Pickup.Net;
using Pickup.Players;
using Pickup.Scenes.InitScene;
using Pickup.Scenes.LobbyScene;
using Pickup.World;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Pickup.Scenes.FieldScene
{
    public sealed partial class FieldSystem : MonoBehaviour
    {
        public static FieldSystem Instance { get; private set; }

        [Header("Components")] 
        [SerializeField] private Tilemap groundMap;
        [SerializeField] private GameObject structureMap;
        public PolygonCollider2D cameraBound;

        [Header("GameObject")] 
        public Player player;
        [SerializeField] private GameObject structurePool;
        [SerializeField] private GameObject originalStructure;

        [Header("GameStatus")] 
        public Field field;
        public bool isMultiplayer = false;
        // network
        [SerializeField]
        private NetworkManager networkManager = NetworkManager.Singleton;

        private void Awake()
        {
            if ((Instance && Instance != this) || !InitSequence.inited)
            {
                Destroy(gameObject);
                return;
            }
            Debug.Log(Assist.tiles["Wall"]);
            field = new Field(new ObjectPool<StructureM>(() =>
            {
                // Create
                var obj = Instantiate(originalStructure, structureMap.transform);
                return obj.GetComponent<StructureM>();
            }, obj =>
            {
                // OnGet
                obj.transform.parent = structureMap.transform;
            }, obj =>
            {
                // OnRelease
                obj.Release();
                obj.transform.parent = structurePool.transform;
            }, obj =>
            {
                // onDestroy
            }));

            Instance = this;
        }

        private void Start()
        {
            if(!InitSequence.inited) return;
            var m = LobbySystem.messenger;
            
            if(m.sceneMoveMode is not SceneMoveMode sceneMoveMode ||
               m.address is not string address ||
               m.multiPlay is not bool multiPlay ) return;

            switch (sceneMoveMode)
            {
                case SceneMoveMode.Create:
                {
                    isMultiplayer = multiPlay;
                    if (multiPlay)
                    {
                        // multi
                        Destroy(player.gameObject);
                    }
                    else
                    {
                        // single
                        PlayerManager.Instance.InitPlayer(player);
                        
                        // init events
                        player.Init();
                    }
                    
                    break;
                }

                case SceneMoveMode.Join:
                {
                    NetworkIO.Connect(address);
                    break;
                }

                case SceneMoveMode.Err:
                {
                    break;
                }

                default:
                {
                    break;
                }
            }
        }

        private void Update()
        {
        }
    }
}