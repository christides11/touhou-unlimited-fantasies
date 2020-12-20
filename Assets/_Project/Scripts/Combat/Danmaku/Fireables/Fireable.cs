using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    public class Fireable : IFireable
    {
        public IFireable Child { get; set; }

        public virtual void Fire(DanmakuConfig config)
        {

        }

        public virtual void SubFire(DanmakuConfig config)
        {
            if(Child == null)
            {
                return;
            }
            Child.Fire(config);
        }
    }
}