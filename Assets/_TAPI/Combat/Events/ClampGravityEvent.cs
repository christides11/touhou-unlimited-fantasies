using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "ClampGravityEvent", menuName = "Attack Events/Clamp Gravity")]
    public class ClampGravityEvent : AttackEvent
    {

        public override bool Evaluate(uint frame, uint endFrame,
            EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
            controller.PhysicsManager.forceGravity.y = Mathf.Clamp(controller.PhysicsManager.forceGravity.y,
                variables.floatVars[0], variables.floatVars[1]);
            return false;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(AttackEventDefinition eventDefinition)
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