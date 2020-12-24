using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class Fireable
    {
        public Fireable Child
        {
            get
            {
                return child;
            }
            set
            {
                child = value;
            }
        }

        [SerializeReference] protected Fireable child;

        public virtual void DrawInspector()
        {

        }

        public virtual void Fire(FireableInfo fireableInfo, DanmakuConfig config)
        {

        }

        public virtual void SubFire(FireableInfo fireableInfo, DanmakuConfig config)
        {
            if(Child == null)
            {
                return;
            }
            Child.Fire(fireableInfo, config);
        }
    }
}