using System;
using System.Collections.Generic;
using Pickup.Net;
using Pickup.Players;
using Pickup.Scenes.InitScene;
using Pickup.Scenes.LobbyScene;
using Pickup.World;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Pickup.Scenes.FieldScene
{
    public sealed partial class FieldSystem : MonoBehaviour
    {
        public static FieldSystem Instance { get; private set; }

        [Header("Components")] 
        public Field field;
        [SerializeField] private Tilemap groundMap;
        [SerializeField] private Tilemap structureMap;
        public PolygonCollider2D cameraBound;

        [Header("GameObject")] 
        public Player player;

        [Header("GameStatus")] 
        public bool isMultiplayer = false;
        // network
        private NetworkManager networkManager => NetworkManager.Singleton;
        // static
        public static bool started = false;
        
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            field = new Field();

            Instance = this;
        }

        private void Start()
        {
            if(started || !InitSequence.inited) return;
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