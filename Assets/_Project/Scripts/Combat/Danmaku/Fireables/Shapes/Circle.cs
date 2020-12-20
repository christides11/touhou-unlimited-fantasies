using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Core;

namespace TUF.Combat.Danmaku.Fireables
{
    [System.Serializable]
    public class Circle : Fireable
    {
        public Range count = 1;
        public Range radius;

        public Circle(Range count, Range radius)
        {
            this.count = count;
            this.radius = radius;
        }

        public override void Fire(DanmakuConfig config)
        {
            float r = radius.GetValue();
            int c = Mathf.RoundToInt(count.GetValue());
            var rotation = config.rotation;
            var origin = config.position;
            config.rotation = rotation;
            for (int i = 0; i < c; i++)
            {
                float angle = i * Mathf.PI * 2 / c;
                float x = Mathf.Cos(angle) * r;
                float z = Mathf.Sin(angle) * r;
                Vector3 pos = origin + new Vector3(x, 0, z);
                
                float angleDegrees = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                
                config.position = Helpers.RotatePointAroundPivot(pos, origin, rotation);
                config.rotation = rot.eulerAngles;
                SubFire(config);
            }
        }
    }
}