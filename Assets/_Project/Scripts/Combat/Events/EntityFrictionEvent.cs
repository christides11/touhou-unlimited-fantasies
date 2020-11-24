using System.Collections.Generic;
using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Events
{
    public class EntityFrictionEvent : AttackEvent
    {
        public bool yFriction;
        public bool xzFriction;

        public override string GetName()
        {
            return "Friction";
        }

        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityManager controller, AttackEventVariables variables)
        {
            TUF.Entities.EntityPhysicsManager physicsManager = (TUF.Entities.EntityPhysicsManager)controller.PhysicsManager;

            if (xzFriction)
            {
                physicsManager.ApplyMovementFriction(variables.floatVars[0]);
            }
            if (yFriction)
            {
                physicsManager.ApplyGravityFriction(variables.floatVars[0]);
            }
            return false;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
        {
            if(eventDefinition.variables.floatVars == null 
                || eventDefinition.variables.floatVars.Count != 1)
            {
                eventDefinition.variables.floatVars = new List<float>(1);
                eventDefinition.variables.floatVars.Add(0);
            }

            xzFriction = EditorGUILayout.Toggle("XZ-Axis", xzFriction);
            yFriction = EditorGUILayout.Toggle("Y-Axis", yFriction);

            eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Friction", 
                eventDefinition.variables.floatVars[0]);
        }
#endif
    }
}