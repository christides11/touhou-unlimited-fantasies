using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TUF.Combat;
using System;

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

        protected override void DrawMenuBar()
        {
            base.DrawMenuBar();
            if (GUILayout.Button("Projectiles"))
            {
                currentMenu = 6;
            }
            if (GUILayout.Button("Meter"))
            {
                currentMenu = 7;
            }
        }

        protected bool followTargetDropdown;
        protected override void DrawGeneralMenu()
        {
            base.DrawGeneralMenu();

            AttackDefinition attack = (AttackDefinition)this.attack;
            // Lockon Target Following
            List<AttackFaceLockonWindow> followWindows = new List<AttackFaceLockonWindow>(attack.faceLockonTargetWindows);
            EditorGUILayout.BeginHorizontal();
            followTargetDropdown = EditorGUILayout.Foldout(followTargetDropdown, "Follow Target Windows", true);
            if (GUILayout.Button("Add"))
            {
                followWindows.Add(new AttackFaceLockonWindow());
            }
            EditorGUILayout.EndHorizontal();
            if (followTargetDropdown)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < followWindows.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        followWindows.RemoveAt(i);
                        continue;
                    }
                    EditorGUILayout.LabelField($"{i}.");
                    EditorGUILayout.EndHorizontal();
                    followWindows[i].startFrame = EditorGUILayout.IntField("Start: ", followWindows[i].startFrame);
                    followWindows[i].endFrame = EditorGUILayout.IntField("End: ", followWindows[i].endFrame);
                    followWindows[i].amount = EditorGUILayout.FloatField("Amount: ", followWindows[i].amount);
                    EditorGUILayout.Space();
                }
                EditorGUI.indentLevel--;
            }

            attack.faceLockonTargetWindows = followWindows;
        }

        protected override void BoxesMenuNavigationBar()
        {
            base.BoxesMenuNavigationBar();
            if (GUILayout.Button("Duplicate", GUILayout.Width(75))) { 
                AttackDefinition attack = (AttackDefinition)this.attack;

                attack.boxGroups.Add(new TUF.Combat.BoxGroup((TUF.Combat.BoxGroup)attack.boxGroups[currentHitboxGroupIndex]));
            }
        }

        protected bool dashCancelWindowsFoldout;
        protected override void DrawCancelWindows()
        {
            EditorGUI.BeginChangeCheck();
            List<Vector2Int> dashCancelWindows = new List<Vector2Int>(((TUF.Combat.AttackDefinition)attack).dashCancelableFrames);

            DrawCancelWindow("Dash Cancel Windows", ref dashCancelWindowsFoldout, ref dashCancelWindows, 180);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(attack, "Changed Cancel Window.");
                ((TUF.Combat.AttackDefinition)attack).dashCancelableFrames = dashCancelWindows; ;
            }

            base.DrawCancelWindows();
        }

        protected override void AddBoxGroup()
        {
            attack.boxGroups.Add(new TUF.Combat.BoxGroup());
        }

        protected override void AddAttackEventDefinition()
        {
            attack.events.Add(new TUF.Combat.AttackEventDefinition());
        }

        protected override void DrawBoxGroup(CAF.Combat.BoxGroup currentGroup)
        {
            base.DrawBoxGroup(currentGroup);
            TUF.Combat.BoxGroup boxGroup = (TUF.Combat.BoxGroup)currentGroup;
        }
    }
}