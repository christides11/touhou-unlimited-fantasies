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
    public class RemoveModifierAction : DanmakuAction
    {
        public List<string> bulletSets = new List<string>();

        public override void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {
            foreach (string d in bulletSets)
            {
                string s = info.id + d;
                info.bulletSets[s].modifiers.Clear();
            }

            info.NextAction();
        }

        public override void DrawInspector()
        {
#if UNITY_EDITOR

#endif
        }
    }
}