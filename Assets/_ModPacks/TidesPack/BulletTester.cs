﻿using CAF.Simulation;
using System.Collections;
using System.Collections.Generic;
using TUF.Combat.Danmaku;
using UnityEngine;
using TUF.Core;
using TUF.Combat;

namespace TidesPack
{
    public class BulletTester : SimObject
    {
        public bool bb = false;

        public DanmakuManager danmakuManager;
        public DanmakuSequence sequence;

        public DanmakuConfig config;

        public EntityTeams team;

        public HitInfo hitInfo;

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
                danmakuManager.Fire(sequence, config, team, hitInfo);
                bb = false;
            }

            danmakuManager.Tick();
        }
    }
}