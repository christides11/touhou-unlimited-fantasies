using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    public class SetLifetimeBPA : BulletPatternAction
    {
        public int lifetime;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            data.currentLifetime = lifetime;
            return false;
        }
    }
}
