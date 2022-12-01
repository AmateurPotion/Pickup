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
        
        GUILayout.Label($"Structure type / {targetComponent.type.GetType().Name}");
        
        m_Queue.Clear();
        
        foreach (var pair in targetComponent.statCollection)
        {
            dynamic value = null;

            switch (targetComponent.statCollection[pair.Key])
            {
                case int obj:
                {
                    value = EditorGUILayout.IntField(pair.Key, obj);
                    break;
                }

                case string obj:
                {
                    value = EditorGUILayout.TextField(pair.Key, obj);
                    break;
                }

                case Object obj:
                {
                    value = EditorGUILayout.ObjectField(pair.Key, obj, obj.GetType(), true);
                    break;
                }
            }
            
            
            
            m_Queue.Enqueue(new(pair.Key, value));
        }

        while (m_Queue.TryDequeue(out var pair))
        {
            targetComponent.statCollection[pair.Key] = pair.Value;
        }
    }
}

#endif