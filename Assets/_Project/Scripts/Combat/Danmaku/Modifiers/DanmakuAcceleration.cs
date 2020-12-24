using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku.Modifiers
{
    public class DanmakuAcceleration : DanmakuModifier
    {
        public Vector3 accel;

        public override void Copy(DanmakuModifier other)
        {
            DanmakuAcceleration da = (DanmakuAcceleration)other;
            accel = da.accel;
        }

        public override void Apply(FireableInfo fi)
        {
            for(int i = 0; i < fi.bullets.Count; i++)
            {
                DanmakuState dc = fi.bulletsConfig[i];
                dc.speed += accel;
                fi.bulletsConfig[i] = dc;
            }
        }
    }
}