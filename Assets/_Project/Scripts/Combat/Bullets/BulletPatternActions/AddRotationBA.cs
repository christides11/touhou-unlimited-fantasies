using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    public class AddRotationBPA : BulletPatternAction
    {
        public Vector3 rotation;

        public override bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            data.currentRotationOffset += rotation;
            return false;
        }
    }
}