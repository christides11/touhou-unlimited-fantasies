﻿using UnityEditor;
using UnityEngine;

namespace TAPI.Combat
{
    [CustomEditor(typeof(AttackDefinition))]
    public class AttackSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Open Editor", GUILayout.Width(Screen.width), GUILayout.Height(45)))
            {
                AttackDefinitionEditorWindow.Init(target as AttackDefinition);
            }

            DrawDefaultInspector();
        }
    }
}