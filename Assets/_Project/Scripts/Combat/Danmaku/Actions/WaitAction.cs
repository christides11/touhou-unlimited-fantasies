using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class WaitAction : DanmakuAction
    {
        public int waitTime = 1;

        public override void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {
            if(info.frame < waitTime)
            {
                info.frame++;
                return;
            }
            info.NextAction();
        }

        public override void DrawInspector()
        {
#if UNITY_EDITOR
            waitTime = EditorGUILayout.IntField("Wait Time", waitTime);
#endif
        }
    }
}