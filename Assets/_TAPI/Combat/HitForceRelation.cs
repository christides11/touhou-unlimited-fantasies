using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    public enum HitForceRelation
    {
        ATTACKER = 0, // Forces relative to the attacker's forward.
        HITBOX = 1, // Forces relative to the hitbox's forward.
        WORLD = 2, // Forces relative to the world's forward.
    }
}