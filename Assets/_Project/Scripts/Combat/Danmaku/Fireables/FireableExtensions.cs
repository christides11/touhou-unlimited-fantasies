using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    public static class FireableExtensions
    {
        static Fireable GetLowestChild(Fireable fireable)
        {
            var last = fireable;
            while (fireable != null)
            {
                last = fireable;
                fireable = fireable.Child as Fireable;
            }
            return last;
        }

        public static Fireable Of(this Fireable fireable, Fireable subemitter)
        {
            if (fireable == null)
            {
                throw new ArgumentNullException("fireable");
            }
            var lowest = GetLowestChild(fireable);
            lowest.Child = subemitter;
            return fireable;
        }
    }
}