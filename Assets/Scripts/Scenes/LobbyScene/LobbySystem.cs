using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Pickup.Scenes.LobbyScene
{
    public sealed class LobbySystem: MonoBehaviour
    {
        // Field Scene Messenger
        public static SceneMoveMode sceneMoveMode { get; private set; } = SceneMoveMode.Err;
        public static string address { get; private set; } = "";
        
        [Header("Components")]
        [SerializeField] private GameObject joinServerPanel;
        [SerializeField] private TMP_InputField addressText;
        [SerializeField] private List<GameObject> deletes = new();

        public void CreateGame()
        {
            sceneMoveMode = SceneMoveMode.CreateGame;
            deletes.ForEach(Destroy);
            SceneManager.LoadSceneAsync("Field", LoadSceneMode.Additive);
        }

        public void LoadGame()
        {
            deletes.ForEach(Destroy);
        }

        public void JoinServer()
        {
            if (!joinServerPanel.activeSelf)
            {
                joinServerPanel.SetActive(true);
            }
        }

        public void JoinServerReal()
        {
            sceneMoveMode = SceneMoveMode.JoinServer;
            address = addressText.text;
            deletes.ForEach(Destroy);
            SceneManager.LoadSceneAsync("Field", LoadSceneMode.Additive);
            
        }

        public void CancelJoinServer()
        {
            joinServerPanel.SetActive(false);
        }
    }

    public enum SceneMoveMode
    {
        CreateGame,
        LoadGame,
        JoinServer,
        Err
    }
}