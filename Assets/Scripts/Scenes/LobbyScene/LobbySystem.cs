using System;
using System.Collections.Generic;
using Pickup.Graphics.UI.Panels;
using Pickup.Utils.Attributes;
using SRF;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace Pickup.Scenes.LobbyScene
{
    public sealed class LobbySystem: MonoBehaviour
    {
        // Field Scene Messenger
        public static dynamic messenger;
        
        [Header("Components")]
        [SerializeField] private new Camera camera;
        [SerializeField] private List<DrawerPanel> panels;

        [Header("Status")] 
        public bool multiPlay = false;

        [SerializeField, GetSet("panelIndex")] private int _panelIndex = 1;
        /// <summary>
        /// 0 - World / 1 - Main / 2 - Character
        /// </summary>
        public int panelIndex
        {
            get => _panelIndex;
            set
            {
                foreach (var p in panels)
                {
                    p.positionIndex = value;
                }
            }
        }
        [SerializeField, GetSet("panelMoveSpeed")] private float _panelMoveSpeed = 1600;

        public float panelMoveSpeed
        {
            get => _panelMoveSpeed;
            set
            {
                _panelMoveSpeed = value;
                foreach (var p in panels)
                {
                    p.moveSpeed = value;
                }
            }
        }

        public void PlaySetup(bool multi)
        {
            multiPlay = multi;
        }

        public void OpenSettings() => Assist.panelManager.setting.Open();
        
        public void StartGame()
        {
            messenger = new
            {
                sceneMoveMode = SceneMoveMode.Create,
                address = "",
                multiPlay
            };
            
            SceneManager.LoadSceneAsync("Field");
        }
    }

    public enum SceneMoveMode
    {
        Create,
        Join,
        Err
    }
}