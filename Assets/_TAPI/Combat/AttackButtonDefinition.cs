using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Combat
{
    [System.Serializable]
    public class AttackButtonDefinition
    {
        public EntityInputs button;
        //Stick
        public Vector2 stickDir;
        public float dirDeviation = 15.0f;
    }
}