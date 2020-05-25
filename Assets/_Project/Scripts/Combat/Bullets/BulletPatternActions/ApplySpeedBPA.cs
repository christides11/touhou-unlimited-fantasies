using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    public class ApplySpeedBPA : BulletPatternAction
    {
        public bool speed;
        public bool angularSpeed;
        public bool localSpeed;
        public bool localAngularSpeed;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            for (int i = 0; i < manager.patternBullets[manager.patterns[group].ID].Count; i++)
            {
                Bullet b = manager.patternBullets[manager.patterns[group].ID][i];
                if (localAngularSpeed)
                {
                    b.SetLocalAngularVelocity(manager.patterns[group].currentLocalAngularSpeed);
                }
                if (localSpeed)
                {
                    b.SetLocalSpeed(manager.patterns[group].currentLocalSpeed);
                }
                if (angularSpeed)
                {
                    b.SetAngularSpeed(manager.patterns[group].currentAngularSpeed);
                }
            }
            return false;
        }
    }
}
