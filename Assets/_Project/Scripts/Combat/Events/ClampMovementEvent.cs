using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Events
{
    public class ClampMovementEvent : AttackEvent
    {

        public override string GetName()
        {
            return "Clamp Movement";
        }

        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityController controller, AttackEventVariables variables)
        {
            controller.PhysicsManager.forceMovement = Vector3.ClampMagnitude(controller.PhysicsManager.forceMovement,
                variables.floatVars[0]);
            return false;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
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