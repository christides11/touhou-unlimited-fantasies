using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    public class CreateBulletPatternBPA : BulletPatternAction
    {
        public BulletPattern pattern;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            manager.AddPattern(pattern);
            return false;
        }
    }
}
