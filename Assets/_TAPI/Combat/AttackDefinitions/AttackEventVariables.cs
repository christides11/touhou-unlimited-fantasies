using System.Collections;
using System.Collections.Generic;
using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Combat
{
    [System.Serializable]
    public class AttackEventVariables
    {
        public List<int> intVars;
        public List<float> floatVars;
        public List<Object> objectVars;
        public List<AnimationCurve> curveVars;
    }
}
