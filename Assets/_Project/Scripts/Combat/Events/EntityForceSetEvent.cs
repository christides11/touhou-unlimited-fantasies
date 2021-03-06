﻿using System.Collections;
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
    public class EntityForceSetEvent : AttackEvent
    {
        public bool xzForce;
        public bool yForce;

        public override string GetName()
        {
            return "Set Forces";
        }

        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityManager controller, AttackEventVariables variables)
        {
            TUF.Entities.EntityPhysicsManager physicsManager = (TUF.Entities.EntityPhysicsManager)controller.PhysicsManager;

            Vector3 f = Vector3.zero;
            if (xzForce)
            {
                f.x = variables.floatVars[0];
                f.z = variables.floatVars[1];
            }
            if (yForce)
            {
                f.y = variables.floatVars[2];
            }

            f = controller.GetVisualBasedDirection(f);

            if (yForce)
            {
                physicsManager.forceGravity.y = f.y;
            }
            if(xzForce)
            {
                f.y = 0;
                physicsManager.forceMovement = f;
            }
            return false;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
        {
            if (eventDefinition.variables.floatVars == null
                || eventDefinition.variables.floatVars.Count != 3)
            {
                eventDefinition.variables.floatVars = new List<float>(3);
                eventDefinition.variables.floatVars.Add(0);
                eventDefinition.variables.floatVars.Add(0);
                eventDefinition.variables.floatVars.Add(0);
            }

            xzForce = EditorGUILayout.Toggle("XZ Force", xzForce);
            yForce = EditorGUILayout.Toggle("Y Force", yForce);

            if (xzForce)
            {
                eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("X Force",
                    eventDefinition.variables.floatVars[0]);
            }
            if (yForce)
            {
                eventDefinition.variables.floatVars[2] = EditorGUILayout.FloatField("Y Force",
                    eventDefinition.variables.floatVars[2]);
            }
            if (xzForce)
            {
                eventDefinition.variables.floatVars[1] = EditorGUILayout.FloatField("Z Force",
                    eventDefinition.variables.floatVars[1]);
            }
        }
#endif
    }
}
