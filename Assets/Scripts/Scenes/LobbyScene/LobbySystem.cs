using System.Collections.Generic;
using SRF;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Pickup.Scenes.LobbyScene
{
    public sealed class LobbySystem: MonoBehaviour
    {
        // Field Scene Messenger
        public static dynamic messenger = new
        {
            sceneMoveMode = SceneMoveMode.Err,
            address = ""
        };
        
        [Header("Components")]
        [SerializeField] private new Camera camera;
        [SerializeField] private GameObject worldPanel;
        [SerializeField] private GameObject serverPanel;

        public void StartGame()
        {
            worldPanel.SetActive(true);
        }
        public void Join()
        {
            serverPanel.SetActive(true);
        }

        public void OpenSettings() => Assist.panelManager.setting.Open();
        
        internal void PrepareSceneMove()
        {
            Destroy(camera);
            gameObject.RemoveComponentIfExists<EventSystem>();
            gameObject.RemoveComponentIfExists<InputSystemUIInputModule>();
        }
    }

    public enum SceneMoveMode
    {
        Create,
        Join,
        Err
    }
}