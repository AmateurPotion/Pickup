using System;
using System.Threading.Tasks;
using Pickup.Contents;
using Pickup.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pickup.Scenes.InitScene
{
    [RequireComponent(typeof(ContentLoader))]
    public class InitSequence : MonoBehaviour
    {
        public GaugeInt loadingProgress = new (0, 100, 0);
        private void Start()
        {
            if (Application.platform == RuntimePlatform.LinuxServer ||
                Application.platform == RuntimePlatform.WindowsServer)
            {
                // Server 
                // Command I/O
                Task.Run(() =>
                {

                });
                
                // Process
                Task.Run(() =>
                {
                });
            }
            else
            {
                //Screen.fullScreen = false;
                GetComponent<ContentLoader>().Load();
                Assist.Initialize();
                SceneManager.LoadSceneAsync("Lobby");
            }
        }
    }
}