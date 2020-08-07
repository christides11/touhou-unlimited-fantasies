using System.Collections;
using System.Collections.Generic;
using TUF.Modding;
using UnityEditor;
using UnityEngine;

namespace TUF.Combat
{
    public class AttackDefinitionEditorWindow : CAF.Combat.AttackDefinitionEditorWindow
    {
        public static void Init(AttackDefinition attack)
        {
            AttackDefinitionEditorWindow window =
                (AttackDefinitionEditorWindow)EditorWindow.GetWindow(typeof(AttackDefinitionEditorWindow));
            window.attack = attack;
            window.Show();
        }

        protected bool floatCancelWindowsFoldout;
        protected override void DrawWindowsMenu()
        {
            base.DrawWindowsMenu();

            AttackDefinition atk = (AttackDefinition)attack;

            if (cancelWindowsFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();

                List<Vector2Int> floatCancelWindows = new List<Vector2Int>(atk.floatCancelFrames);

                DrawCancelWindow("Float Cancel Windows", ref floatCancelWindowsFoldout, ref floatCancelWindows, 180);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(atk, "Changed Cancel Window.");
                    atk.floatCancelFrames = floatCancelWindows;
                }
                EditorGUI.indentLevel--;
            }
        }

        protected override void DrawBoxGroupHitOptions(CAF.Combat.BoxGroup currentGroup)
        {
            base.DrawBoxGroupHitOptions(currentGroup);

            BoxGroup boxGroup = (BoxGroup)currentGroup;

            EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
            boxGroup.hitSound = (ModObjectLink)EditorGUILayout.ObjectField("Hit Sound", boxGroup.hitSound, typeof(ModObjectLink), false);
            EditorGUILayout.Space();
        }
    }
}