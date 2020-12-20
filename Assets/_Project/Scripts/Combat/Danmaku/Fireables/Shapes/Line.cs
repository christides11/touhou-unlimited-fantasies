using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Core;

namespace TUF.Combat.Danmaku.Fireables
{
    public class Line : Fireable
    {
        public Range count = 1;
        public Range3 deltaSpeed;

        public Line(Range count, Range3 deltaSpeed)
        {
            this.count = count;
            this.deltaSpeed = deltaSpeed;
        }

        public override void Fire(DanmakuConfig config)
        {
            int c = Mathf.RoundToInt(count.GetValue());
            var dSpeed = deltaSpeed.GetValue();
            for (var i = 0; i < c; i++)
            {
                config.speed += dSpeed;
                SubFire(config);
            }
        }
    }
}