using System.Collections.Generic;
using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "EntityFrictionEvent", menuName = "Attack Events/Friction")]
    public class EntityFrictionEvent : AttackEvent
    {
        public bool yFriction;
        public bool xzFriction;

        public override void Evaluate(uint frame, uint endFrame, 
            EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
            if (xzFriction)
            {
                controller.PhysicsManager.ApplyMovementFriction(variables.floatVars[0]);
            }
            if (yFriction)
            {
                controller.PhysicsManager.ApplyGravityFriction(variables.floatVars[0]);
            }
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(AttackEventDefinition eventDefinition)
        {
            if(eventDefinition.variables.floatVars == null 
                || eventDefinition.variables.floatVars.Count != 1)
            {
                eventDefinition.variables.floatVars = new List<float>(1);
                eventDefinition.variables.floatVars.Add(0);
            }

            eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Friction", 
                eventDefinition.variables.floatVars[0]);
        }
#endif
    }
}