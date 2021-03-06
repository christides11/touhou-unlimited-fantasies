﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class FireAction : DanmakuAction
    {
        [System.NonSerialized] protected Dictionary<string, Type> actionTypes = new Dictionary<string, Type>();

        [SerializeReference] public Fireable fireable;

        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        public string id;

        public HitInfo hitInfo;

        public override void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {
            if(info.bulletSets.Count <= info.currentSet)
            {
                FireableInfo fi = new FireableInfo();
                fi.team = info.team;
                fi.hitInfo = hitInfo;
                fi.hitboxOwner = danmakuManager.gameObject;
                info.bulletSets.Add(info.id + id, fi);
            }

            DanmakuConfig dc = info.baseConfig;
            if (dc.startPoint)
            {
                dc.position += dc.startPoint.position;
                dc.rotation += dc.startPoint.eulerAngles;
            }
            dc.position += positionOffset;
            dc.rotation += rotationOffset;
            fireable.Fire(info.bulletSets[info.id + id], dc);

            info.NextAction();
        }

        public override void DrawInspector()
        {
#if UNITY_EDITOR
            DrawFireable(fireable);

            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                // Fill dictionary.
                actionTypes.Clear();
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var givenType in a.GetTypes())
                    {
                        if (givenType.IsSubclassOf(typeof(Fireable)))
                        {
                            actionTypes.Add(givenType.FullName, givenType);
                        }
                    }
                }

                // ?
                GenericMenu menu = new GenericMenu();

                foreach (string t in actionTypes.Keys)
                {
                    string destination = t.Replace('.', '/');
                    menu.AddItem(new GUIContent(destination), true, OnActionSelected, t);
                }
                menu.ShowAsContext();
            }
#endif
        }

        private void DrawFireable(Fireable fireable)
        {
#if UNITY_EDITOR
            if (fireable == null)
            {
                return;
            }

            GUILayout.Label(fireable.GetType().Name);
            fireable.DrawInspector();

            if (fireable.Child != null)
            {
                GUILayout.Space(10);
                GUILayout.Label("Of...");
                DrawFireable((Fireable)fireable.Child);
            }
#endif
        }

        private void OnActionSelected(object t)
        {
            if(fireable == null)
            {
                fireable = (Fireable)Activator.CreateInstance(actionTypes[(string)t]);
                return;
            }

            Fireable f = fireable;
            while(f.Child != null)
            {
                f = (Fireable)f.Child;
            }

            f.Child = (Fireable)Activator.CreateInstance(actionTypes[(string)t]);
        }
    }
}