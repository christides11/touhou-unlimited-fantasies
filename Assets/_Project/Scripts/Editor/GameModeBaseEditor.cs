using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TUF.GameMode
{
    [CustomEditor(typeof(GameModeBase), true)]
    public class GameModeBaseEditor : Editor
    {
        GameModeComponentHolder gmch;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GameModeBase gmb = target as GameModeBase;
            base.OnInspectorGUI();

            gmch = (GameModeComponentHolder)EditorGUILayout.ObjectField("Add", gmch, typeof(GameModeComponentHolder), true);

            if (gmch)
            {
                Undo.RecordObject(target, $"Added Component to Game Mode.");
                System.Type componentDataType = gmch.GetGameModeComponent().GetComponentData().GetType();
                gmb.componentPrefabs.Add(gmch);
                gmb.componentsData.Add((GameModeComponentData)Activator.CreateInstance(componentDataType));
                gmch = null;
            }

            if(gmb.componentPrefabs.Count != gmb.componentsData.Count)
            {
                gmb.componentPrefabs.Clear();
                gmb.componentsData.Clear();
            }

            for(int i = 0; i < gmb.componentPrefabs.Count; i++)
            {
                if (GUILayout.Button("X"))
                {
                    gmb.componentPrefabs.RemoveAt(i);
                    gmb.componentsData.RemoveAt(i);
                    continue;
                }

                EditorGUILayout.LabelField(gmb.componentPrefabs[i].name);
                gmb.componentsData[i].DrawInspectorUI();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}