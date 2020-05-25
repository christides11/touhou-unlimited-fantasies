using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    [System.Serializable]
    public class BulletPatternGroup
    {
        public int spawnFrame;
        public BulletPattern bulletPattern;
        public bool attachToEntity = false;
        public Vector3 offset;
    }
}