﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace TUF.Combat.Danmaku
{
    [CustomEditor(typeof(DanmakuSequence), true)]
    public class DanmakuSequenceEditor : Editor
    {
        protected Dictionary<string, Type> actionTypes = new Dictionary<string, Type>();

        public virtual void OnEnable()
        {
            actionTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(DanmakuAction)))
                    {
                        actionTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            DanmakuSequence dSequence = (DanmakuSequence)target;

            for(int i = 0; i < dSequence.sequence.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("^", GUILayout.Width(20))
                    && i != 0)
                {
                    DanmakuAction da = dSequence.sequence[i-1];
                    dSequence.sequence[i - 1] = dSequence.sequence[i];
                    dSequence.sequence[i] = da;
                    return;
                }
                if (GUILayout.Button("v", GUILayout.Width(20))
                    && i != dSequence.sequence.Count-1)
                {
                    DanmakuAction da = dSequence.sequence[i + 1];
                    dSequence.sequence[i + 1] = dSequence.sequence[i];
                    dSequence.sequence[i] = da;
                    return;
                }
                if(GUILayout.Button("dup", GUILayout.Width(35)))
                {
                    DanmakuAction da = (DanmakuAction)Activator.CreateInstance(dSequence.sequence[i].GetType());
                    da.Copy(dSequence.sequence[i]);
                    dSequence.sequence.Insert(i+1, da);
                    return;
                }
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    dSequence.sequence.RemoveAt(i);
                    return;
                }
                GUILayout.Label(dSequence.sequence[i].GetType().Name);
                GUILayout.EndHorizontal();
                dSequence.sequence[i].DrawInspector();
                GUILayout.Space(20);
                DrawUILine(Color.white);
            }


            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string t in actionTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnActionSelected, t);
                }
                menu.ShowAsContext();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(dSequence);
            }
        }

        private void OnActionSelected(object t)
        {
            DanmakuSequence dSequence = (DanmakuSequence)target;
            dSequence.sequence.Add((DanmakuAction)Activator.CreateInstance(actionTypes[(string)t]));
        }

        public void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
    }
}