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

        public DanmakuManager danmakuManager;
        public DanmakuSequence sequence;

        public DanmakuConfig config;

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
                danmakuManager.Fire(sequence, config);
                bb = false;
            }

            danmakuManager.Tick();
        }
    }
}