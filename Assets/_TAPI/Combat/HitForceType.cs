using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    public enum HitForceType
    {
        SET = 0, // Force is set and based on a given forward direction
        AWAY = 1 // Force is based on what direction the hit object is compared to the hitbox.
    }
}