using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using TUF.Combat.Danmaku;
using UnityEngine;
using TUF.Core;

namespace TidesPack
{
    public class BulletTester : SimObject
    {
        public bool bb = false;

        public BulletSet bulletSet = new BulletSet();

        public DanmakuConfig config;
        IFireable fireable;

        public int lineCount = 5;
        public Range3 lineSpeed;

        public float circleCOunt = 5;
        public float circleRadius = 4;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                bb = true;
            }
        }

        public override void SimUpdate()
        {
            if (bb)
            {
                config.position = transform.position;
                config.rotation = transform.eulerAngles;
                fireable = new TUF.Combat.Danmaku.Fireables.Line(lineCount, lineSpeed)
                    .Of(new TUF.Combat.Danmaku.Fireables.Circle(circleCOunt, circleRadius))
                    .Of(bulletSet);
                fireable.Fire(config);
                bb = false;
            }
            bulletSet.Update();
        }
    }
}