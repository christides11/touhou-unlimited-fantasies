using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Danmaku.Fireables
{
    public class Line : Fireable
    {
        public Range count = 1;
        public Range3 deltaSpeed;

        public Line()
        {

        }

        public Line(Range count, Range3 deltaSpeed)
        {
            this.count = count;
            this.deltaSpeed = deltaSpeed;
        }

        public override void DrawInspector()
        {
#if UNITY_EDITOR
            float countMin = count.Min;
            float countMax = count.Max;
            countMin = EditorGUILayout.FloatField("Child Count (Min)", countMin);
            countMax = EditorGUILayout.FloatField("Child Count (Max)", countMax);
            count.Set(countMin, countMax);

#endif
        }

        public override void Fire(FireableInfo fireableInfo, DanmakuConfig config)
        {
            int c = Mathf.RoundToInt(count.GetValue());
            var dSpeed = deltaSpeed.GetValue();
            for (var i = 0; i < c; i++)
            {
                config.speed += dSpeed;
                SubFire(fireableInfo, config);
            }
        }
    }
}