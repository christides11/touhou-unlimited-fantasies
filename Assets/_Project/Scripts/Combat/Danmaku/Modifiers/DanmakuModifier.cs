using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku.Modifiers
{
    public class DanmakuModifier
    {
        public DanmakuModifier()
        {

        }

        public DanmakuModifier(DanmakuModifier other)
        {
            Copy(other);
        }

        public virtual void Copy(DanmakuModifier other)
        {

        }


        public virtual void Apply(FireableInfo fi)
        {

        }

        public virtual void Tick(FireableInfo fi)
        {

        }
    }
}