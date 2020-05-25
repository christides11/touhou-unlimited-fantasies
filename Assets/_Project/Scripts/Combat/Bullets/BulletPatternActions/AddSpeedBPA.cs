using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    public class AddSpeedBPA : BulletPatternAction
    {
        public Vector3 speed;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            data.currentSpeed += speed;
            return false;
        }
    }
}