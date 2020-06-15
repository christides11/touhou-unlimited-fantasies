using System.Collections;
using System.Collections.Generic;
using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;
using TUF.Core;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Events
{
    public class EntityForceCurveEvent : AttackEvent
    {
        public bool xForce;
        public bool yForce;
        public bool zForce;

        public override string GetName()
        {
            return "Force Curve";
        }

        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityController controller, AttackEventVariables variables)
        {
            Vector3 f = Vector3.zero;
            float percent = (float)frame / (float)endFrame;
            if (xForce)
            {
                f.x = variables.curveVars[0].Evaluate(percent)
                    * variables.floatVars[0];
            }
            if (yForce)
            {
                f.y = variables.curveVars[1].Evaluate(percent)
                    * variables.floatVars[1];
            }
            if (zForce)
            {
                f.z = variables.curveVars[2].Evaluate(percent)
                    * variables.floatVars[2];
            }

            float tempY = f.y;
            f = controller.GetVisualBasedDirection(f);

            // Set Mode
            if (variables.intVars[0] == 0)
            {
                if (yForce)
                {
                    controller.PhysicsManager.forceGravity.y = tempY;
                }
                if (xForce || zForce)
                {
                    f.y = 0;
                    controller.PhysicsManager.forceMovement = f;
                }
            }
            else
            {
                if (yForce)
                {
                    controller.PhysicsManager.forceGravity.y += f.y;
                }
                if (xForce || zForce)
                {
                    f.y = 0;
                    controller.PhysicsManager.forceMovement += f;
                }
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
            if (eventDefinition.variables.curveVars == null
                || eventDefinition.variables.curveVars.Count != 3)
            {
                eventDefinition.variables.curveVars = new List<AnimationCurve>(3);
                eventDefinition.variables.curveVars.Add(new AnimationCurve());
                eventDefinition.variables.curveVars.Add(new AnimationCurve());
                eventDefinition.variables.curveVars.Add(new AnimationCurve());
            }
            if(eventDefinition.variables.intVars == null
                || eventDefinition.variables.intVars.Count != 1)
            {
                eventDefinition.variables.intVars = new List<int>(1);
                eventDefinition.variables.intVars.Add(0);
            }

            ForceType ft = (ForceType)eventDefinition.variables.intVars[0];
            ft = (ForceType)EditorGUILayout.EnumPopup("Force Mode", 
                (ForceType)eventDefinition.variables.intVars[0]);
            eventDefinition.variables.intVars[0] = (int)ft;

            if (xForce)
            {
                eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("X Curve Multiplier",
                    eventDefinition.variables.floatVars[0]);
                eventDefinition.variables.curveVars[0] = EditorGUILayout.CurveField("X Curve",
                    eventDefinition.variables.curveVars[0]);
            }
            if (yForce)
            {
                eventDefinition.variables.floatVars[1] = EditorGUILayout.FloatField("Y Curve Multiplier",
                    eventDefinition.variables.floatVars[1]);
                eventDefinition.variables.curveVars[1] = EditorGUILayout.CurveField("Y Curve",
                    eventDefinition.variables.curveVars[1]);
            }
            if (zForce)
            {
                eventDefinition.variables.floatVars[2] = EditorGUILayout.FloatField("Z Curve Multiplier",
                    eventDefinition.variables.floatVars[2]);
                eventDefinition.variables.curveVars[2] = EditorGUILayout.CurveField("Z Curve",
                    eventDefinition.variables.curveVars[2]);
            }
        }
#endif
    }
}