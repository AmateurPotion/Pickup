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
            
            switch (LobbySystem.sceneMoveMode)
            {
                case SceneMoveMode.CreateGame:
                {
                    break;
                }

                case SceneMoveMode.LoadGame:
                {
                    break;
                }
                
                case SceneMoveMode.JoinServer:
                {
                    NetworkIO.Connect(LobbySystem.address);
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
            if (Input.GetKeyDown(KeyCode.H) && LobbySystem.sceneMoveMode != SceneMoveMode.JoinServer && 
                !networkManager.IsHost && !networkManager.IsClient)
            {
                NetworkIO.Host(out _);
            }
        }
    }
}