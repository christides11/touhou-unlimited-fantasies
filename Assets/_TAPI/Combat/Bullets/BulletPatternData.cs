﻿using System.Collections.Generic;
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
        public Vector3 currentLocalSpeed;
        public Vector3 currentAngularSpeed;
        public Vector3 currentLocalAngularSpeed;
        public Vector3 currentOffset;

        public Dictionary<string, float> floatVariables = new Dictionary<string, float>();

        public BulletPatternData(int ID, BulletPattern bulletPattern, Transform relativeTo)
        {
            this.ID = ID;
            this.bulletPattern = bulletPattern;
            this.relativeTo = relativeTo;
        }
    }
}