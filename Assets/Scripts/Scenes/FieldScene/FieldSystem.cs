using System;
using System.Collections.Generic;
using Pickup.Net;
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
            if(started) return;
            
            if(LobbySystem.messenger.sceneMoveMode is not SceneMoveMode sceneMoveMode ||
               LobbySystem.messenger.address is not string address) return;
            
            switch (sceneMoveMode)
            {
                case SceneMoveMode.Create:
                {
                    
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
            }
            
            SceneManager.UnloadSceneAsync("Lobby");
        }

        private void Update()
        {
        }
    }
}