using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class AddSequenceAction : DanmakuAction
    {
        public DanmakuSequence sequence;

        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        public override void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {
            DanmakuConfig dc = info.baseConfig;
            dc.position += positionOffset;
            dc.rotation += rotationOffset;
            danmakuManager.Fire(sequence, dc);
            info.NextAction();
        }

        public override void DrawInspector()
        {
#if UNITY_EDITOR
            sequence = (DanmakuSequence)EditorGUILayout.ObjectField("Sequence:", sequence, typeof(DanmakuSequence), false);
            rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset:", rotationOffset);
#endif
        }
    }
}