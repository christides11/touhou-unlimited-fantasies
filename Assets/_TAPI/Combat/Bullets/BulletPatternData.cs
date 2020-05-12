using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat.Bullets
{
    [System.Serializable]
    public class BulletPatternData
    {
        public int ID;
        public int patternPosition;
        public BulletPattern bulletPattern;
        public Transform relativeTo;

        public Vector3 currentSpeed;

        public Dictionary<string, float> floatVariables = new Dictionary<string, float>();

        public BulletPatternData(BulletPattern bulletPattern, Transform relativeTo)
        {
            this.bulletPattern = bulletPattern;
            this.relativeTo = relativeTo;
        }
    }
}