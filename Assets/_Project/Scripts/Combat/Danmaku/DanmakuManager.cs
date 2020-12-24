using System;
using System.Collections;
using System.Collections.Generic;
using TUF.Combat.Danmaku.Modifiers;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    public class DanmakuManager : MonoBehaviour
    {
        public List<DanmakuSequenceInfo> sequences = new List<DanmakuSequenceInfo>();

        public virtual void Tick()
        {
            for(int i = 0; i < sequences.Count; i++)
            {
                sequences[i].Tick(this);
            }
        }

        public virtual void Fire(DanmakuSequence sequence, DanmakuConfig config, string id = "")
        {
            sequences.Add(new DanmakuSequenceInfo(sequence, config, id));
        }
    }
}