using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Core
{
    [System.Serializable]
    public class CombatVariables
    {
        public GameObject hitbox;
        public GameObject detectbox;
        public float tumbleMinimumMagnitude;
        public float groundBounceForce;
    }
}