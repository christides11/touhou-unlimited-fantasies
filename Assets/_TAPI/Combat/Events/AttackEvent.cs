using System.Collections;
using System.Collections.Generic;
using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat
{
    public class AttackEvent : ScriptableObject
    {
        public virtual void Evaluate(uint frame, uint endFrame, EntityAttack attackState, EntityController controller,
            AttackEventVariables variables)
        {

        }

#if UNITY_EDITOR
        public virtual void DrawEventVariables(AttackEventDefinition eventDefinition)
        {
        }
#endif
    }
}