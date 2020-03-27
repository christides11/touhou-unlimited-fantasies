using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    public enum ShapeType
    {
        Rectangle, Circle
    }

    [System.Serializable]
    public class HitboxDefinition
    {
        public ShapeType shape;
        public Vector3 offset;
        public Vector3 size;
        public Vector3 rotation;
        public float radius;
    }
}