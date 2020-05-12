using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat.Bullets
{
    public class SetLocalSpeedBPA : BulletPatternAction
    {
        public Vector3 localSpeed;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            data.currentLocalSpeed = localSpeed;
            return false;
        }
    }
}
