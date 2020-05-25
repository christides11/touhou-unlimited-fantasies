using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    [System.Serializable]
    public class ClearBPA : BulletPatternAction
    {
        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            manager.ClearBullets(group);
            return false;
        }
    }
}