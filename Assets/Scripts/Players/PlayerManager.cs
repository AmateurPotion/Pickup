using System;
using System.IO;
using Cinemachine;
using Pickup.Net;
using Pickup.Net.FieldBuilder;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Pickup.Players
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        [Header("Components")]
        public Player player;
        public Controller controller;
        public CinemachineVirtualCamera virtualCamera;
        [SerializeField] private LineRenderer aimLine;
        [SerializeField] private Image healthImage;
        [SerializeField] private CircleMenu circleMenu;

        // [Header("Network")]
        public NetworkManager networkManager => NetworkManager.Singleton;

        // Unity Events
        private void Awake()
        {
            Instance = this;
            circleMenu.gameObject.SetActive(false);
        }

        private bool fullHealth = true;
        public void InitPlayer(Player self)
        {
            if(player) return;
            
            //aimLine.gameObject.SetActive(true);
            player = self;
            self.health.onChange.AddListener(_ =>
            {
                var ratio = self.health.GetFillRatio();
                healthImage.fillAmount = ratio;
                fullHealth = ratio > 0.99f;
                if (!fullHealth)
                {
                    healthImage.color = new Color(healthImage.color.r, healthImage.color.g, healthImage.color.b, 1 - ratio);
                }
            });
            aimLine = self.aim;
            virtualCamera.Follow = self.transform;
            self.name = Vars.Instance.uuid;
        }

        private void Update()
        {
            if (player && aimLine.gameObject.activeSelf)
            {
                aimLine.SetPositions(new Vector3[] {
                    transform.position, controller.worldMousePoint
                });
            }
            
            if (Input.GetKeyDown(KeyCode.C) && NetworkIO.Connect("127.0.0.1"))
            {
                //InitPlayer(networkManager.LocalClient.PlayerObject.GetComponent<Player>());
            }
            
            if (Input.GetKeyDown(KeyCode.H) && NetworkIO.Host(out var hostIO))
            {
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                File.Create(Path.Combine(Application.dataPath, "test.txt"));
            }
        }

        private void FixedUpdate()
        {
            if (fullHealth)
            {
                healthImage.color = new Color(healthImage.color.r, healthImage.color.g, healthImage.color.b,
                        healthImage.color.a - 1 / 255f);
            }
        }

        // Controller
        private void OnClick(InputValue value)
        {
            if (value.Get<float>().Equals(0) && Camera.main && circleMenu)
            {
                circleMenu.gameObject.SetActive(false);
            }
        }
        
        private void OnRightClick(InputValue value)
        {
            if (value.Get<float>().Equals(0) && Camera.main)
            {
                float Get(float val) => val > 0 ? (int)val + 0.5f : -((int)Math.Abs(val) + 0.5f);
                
                if(circleMenu && !circleMenu.gameObject.activeSelf) 
                    circleMenu.OpenMenu(Camera.main.WorldToScreenPoint(new Vector3(Get(controller.worldMousePoint.x), Get(controller.worldMousePoint.y))));
            }
        }

        // Inventory method
        
        // Field method
        public bool TryGetMouseTile(Vector2Int position, out TileData tileData)
        {
            tileData = new TileData();
            return false;
        }
        
    }
}