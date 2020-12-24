using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku.Modifiers
{
    public class DanmakuDampen : DanmakuModifier
    {
        public float damp;

        public override void Copy(DanmakuModifier other)
        {
            DanmakuDampen da = (DanmakuDampen)other;
            damp = da.damp;
        }

        public override void Tick(FireableInfo fi)
        {
            for (int i = 0; i < fi.bullets.Count; i++)
            {
                DanmakuState ds = fi.bulletsConfig[i];
                if (ds.speed.z > 0)
                {
                    ds.speed.z -= damp;
                    if (ds.speed.z < 0)
                    {
                        ds.speed.z = 0;
                    }
                }
                fi.bulletsConfig[i] = ds;
            }
        }
    }
}