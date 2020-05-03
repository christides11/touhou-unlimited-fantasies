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
        /// <summary>
        /// Proceses the event.
        /// </summary>
        /// <param name="frame">The frame the event is on relative to it's start.</param>
        /// <param name="endFrame">The last frame of the event, relative to it's start.</param>
        /// <param name="attackState">The attack state using this event.</param>
        /// <param name="controller">The controller using this event.</param>
        /// <param name="variables"></param>
        /// <returns>True if the attack state should cancel.</returns>
        public virtual bool Evaluate(uint frame, uint endFrame, EntityAttack attackState, EntityController controller,
            AttackEventVariables variables)
        {
            return false;
        }

#if UNITY_EDITOR
        public virtual void DrawEventVariables(AttackEventDefinition eventDefinition)
        {
        }
#endif
    }
}