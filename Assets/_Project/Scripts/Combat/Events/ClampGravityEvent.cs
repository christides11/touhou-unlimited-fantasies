﻿using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Events
{
    public class ClampGravityEvent : AttackEvent
    {

        public override string GetName()
        {
            return "Clamp Gravity";
        }

        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityController controller, AttackEventVariables variables)
        {
            controller.PhysicsManager.forceGravity.y = Mathf.Clamp(controller.PhysicsManager.forceGravity.y,
                variables.floatVars[0], variables.floatVars[1]);
            return false;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
        {
            if (eventDefinition.variables.floatVars == null
                || eventDefinition.variables.floatVars.Count != 2)
            {
                eventDefinition.variables.floatVars = new List<float>(2);
                eventDefinition.variables.floatVars.Add(0);
                eventDefinition.variables.floatVars.Add(0);
            }

            eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Min", eventDefinition.variables.floatVars[0]);
            eventDefinition.variables.floatVars[1] = EditorGUILayout.FloatField("Max", eventDefinition.variables.floatVars[1]);
        }
#endif
    }
}