﻿using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "ClampMovementEvent", menuName = "Attack Events/Clamp Movement")]
    public class ClampMovementEvent : AttackEvent
    {

        public override bool Evaluate(uint frame, uint endFrame,
            EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
            controller.PhysicsManager.forceMovement = Vector3.ClampMagnitude(controller.PhysicsManager.forceMovement, 
                variables.floatVars[0]);
            return false;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(AttackEventDefinition eventDefinition)
        {
            if (eventDefinition.variables.floatVars == null
                || eventDefinition.variables.floatVars.Count != 1)
            {
                eventDefinition.variables.floatVars = new List<float>(1);
                eventDefinition.variables.floatVars.Add(0);
            }

            eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Max Length", eventDefinition.variables.floatVars[0]);
        }
#endif
    }
}