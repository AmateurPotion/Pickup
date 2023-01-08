#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Pickup.Editor.Tools
{
    public class AssetMemoryViewer : EditorWindow
    {
        [MenuItem("Viewer/Asset Memory Viewer")]
        private static void ShowWindow()
        {
            var window = GetWindow<AssetMemoryViewer>("Asset Memory Viewer");
            window.Show();
        }

        private void OnGUI()
        {
            var bold = new GUIStyle();

            bold.fontSize = 20;
            //Assist.contents.structures
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Structures", bold);
                
                EditorGUILayout.LabelField("Tiles", bold);
            }
            else
            {
                EditorGUILayout.LabelField("실행 중 아님");
            }
            
        }
    }
}

#endif