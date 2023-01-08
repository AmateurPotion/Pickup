using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pickup.Editor.Tools
{
    public class AssetUtility : EditorWindow
    {
        private Object obj = null;
        
        [MenuItem("Assets/Assets Utility")]
        private static void ShowWindow()
        {
            var window = GetWindow<AssetUtility>("Asset Utility");
            window.Show();
        }

        private void OnGUI()
        {
            obj = EditorGUILayout.ObjectField("Resource Type", obj, typeof(MonoScript), false);
            
            if (GUILayout.Button("Find Object") && obj != null)
            {
                var list = Resources.FindObjectsOfTypeAll(((MonoScript)obj).GetClass());
                foreach (var o in list)
                {
                    Debug.Log($"{o.name} : {o.GetInstanceID()}");
                }
            }
        }
    }
}