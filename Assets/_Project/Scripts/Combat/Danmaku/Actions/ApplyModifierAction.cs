using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Combat.Danmaku.Modifiers;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class ApplyModifierAction : DanmakuAction
    {
        [System.NonSerialized] protected Dictionary<string, Type> actionTypes = new Dictionary<string, Type>();

        public List<string> bulletSets = new List<string>();

        [SerializeReference] public DanmakuModifier modifier;

        public override void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {

            foreach(string d in bulletSets) {
                string s = info.id + d;
                DanmakuModifier dm = (DanmakuModifier)Activator.CreateInstance(modifier.GetType());
                dm.Copy(modifier);
                info.bulletSets[s].modifiers.Add(dm);
                dm.Apply(info.bulletSets[s]);
            }

            info.NextAction();
        }

        public override void DrawInspector()
        {
#if UNITY_EDITOR
            if (modifier != null)
            {
                EditorGUILayout.LabelField($"Current Modifier: {modifier.GetType().Name}");
            }

            if (GUILayout.Button("Set", GUILayout.Width(50)))
            {
                // Fill dictionary.
                actionTypes.Clear();
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var givenType in a.GetTypes())
                    {
                        if (givenType.IsSubclassOf(typeof(DanmakuModifier)))
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

        private void OnActionSelected(object t)
        {
            modifier = (DanmakuModifier)Activator.CreateInstance(actionTypes[(string)t]);
        }
    }
}