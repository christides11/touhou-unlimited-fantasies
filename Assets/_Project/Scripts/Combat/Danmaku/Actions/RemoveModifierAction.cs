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
        public int bulletSetIndex = -1;

        public override void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {
            int min = bulletSetIndex;
            int max = bulletSetIndex + 1;
            if (bulletSetIndex == -1)
            {
                min = 0;
                max = info.bulletSets.Count;
            }

            for (int i = min; i < max; i++)
            {
                info.bulletSets[i].modifiers.Clear();
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