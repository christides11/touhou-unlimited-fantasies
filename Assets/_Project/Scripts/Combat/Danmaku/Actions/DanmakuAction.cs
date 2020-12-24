using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class DanmakuAction
    {
        public virtual void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {

        }

        public virtual void DrawInspector()
        {

        }
    }
}