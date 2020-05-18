using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat.Bullets
{
    public class SetAngularSpeedBPA : BulletPatternAction
    {
        public Vector3 angularSpeed;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            data.currentAngularSpeed = angularSpeed;
            return false;
        }
    }
}
