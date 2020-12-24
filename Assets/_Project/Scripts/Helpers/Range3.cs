using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Core
{
    [System.Serializable]
    public struct Range3
    {
        public Range x, y, z;

        public Range3(Range x, Range y, Range z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 GetValue()
        {
            return new Vector3(x.GetValue(), y.GetValue(), z.GetValue());
        }

        public static Range3 operator +(Range3 lhs, Range3 rhs)
        {
            return new Range3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static Range3 operator +(Range3 lhs, Vector3 rhs)
        {
            return new Range3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static Range3 operator *(Range3 lhs, float rhs)
        {
            return new Range3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }
    }
}