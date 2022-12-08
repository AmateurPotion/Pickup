#if UNITY_EDITOR

using System.Collections.Generic;
using Pickup.Scenes.FieldScene;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StructureM)), CanEditMultipleObjects]
public class StructureMEditor : Editor
{
    private readonly Queue<KeyValuePair<string, dynamic>> m_Queue = new();
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var targetComponent = (StructureM)target;
        var typeName = targetComponent.type ? targetComponent.type.GetType().Name : "undefined";
        
        GUILayout.Label($"Structure type / {typeName}");
        
        m_Queue.Clear();
        
        foreach (var pair in targetComponent.tags)
        {
            dynamic value = targetComponent.tags[pair.Key] switch
            {
                int obj => EditorGUILayout.IntField(pair.Key, obj),
                string obj => EditorGUILayout.TextField(pair.Key, obj),
                Object obj => EditorGUILayout.ObjectField(pair.Key, obj, obj.GetType(), true),
                _ => null
            };

            m_Queue.Enqueue(new(pair.Key, value));
        }

        while (m_Queue.TryDequeue(out var pair))
        {
            targetComponent.tags[pair.Key] = pair.Value;
        }
    }
}

#endif