using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Malee.List;
using System;

namespace TUF.Stages
{
    [CustomEditor(typeof(SpawnPointManager))]
    public class SpawnPointManagerEditor : Editor
    {
        private ReorderableList list1;

        void OnEnable()
        {

            list1 = new ReorderableList(serializedObject.FindProperty("spawnPoints"));
            list1.elementNameProperty = "myEnum";
            //list1.onRemoveCallback += OnSpawnPointRemoved;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SpawnPointManager spm = target as SpawnPointManager;

            if(GUILayout.Button("Create Spawn Point"))
            {
                GameObject sp = new GameObject("spawnPoint");
                sp.transform.SetParent(spm.transform);
                list1.AddItem<Transform>(sp.transform);
            }

            //draw the list using GUILayout, you can of course specify your own position and label
            list1.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}