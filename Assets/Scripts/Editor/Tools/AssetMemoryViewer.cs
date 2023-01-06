using System;
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
                foreach (var structure in Assist.contents.structures)
                {
                    EditorGUILayout.ObjectField(structure.Key, structure.Value, typeof(GameObject), false);
                }
                
                EditorGUILayout.LabelField("Tiles", bold);
                foreach (var structure in Assist.contents.tiles)
                {
                    EditorGUILayout.ObjectField(structure.Key, structure.Value, typeof(RuleTile), false);
                }
            }
            else
            {
                EditorGUILayout.LabelField("실행 중 아님");
            }
            
        }
    }
}