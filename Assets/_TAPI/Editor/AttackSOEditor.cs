using UnityEditor;
using UnityEngine;

namespace TAPI.Combat
{
    [CustomEditor(typeof(AttackSO))]
    public class AttackSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Open Editor", GUILayout.Width(Screen.width), GUILayout.Height(45)))
            {
                AttackEditorWindow.Init(target as AttackSO);
            }

            DrawDefaultInspector();
        }
    }
}