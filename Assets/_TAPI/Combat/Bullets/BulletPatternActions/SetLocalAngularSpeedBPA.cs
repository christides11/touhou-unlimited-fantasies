using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat.Bullets
{
    public class SetLocalAngularSpeedBPA : BulletPatternAction
    {
        public Vector3 localAngularSpeed;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            data.currentLocalAngularSpeed = localAngularSpeed;
            return false;
        }
    }
}
