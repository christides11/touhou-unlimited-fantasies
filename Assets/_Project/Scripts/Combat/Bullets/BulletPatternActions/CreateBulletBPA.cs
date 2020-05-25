using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    [System.Serializable]
    public class CreateBulletBPA : BulletPatternAction
    {
        public Bullet bullet;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            manager.CreateBullet(group, bullet);
            return false;
        }
    }
}