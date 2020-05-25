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
    public class EntityApplyGravityEvent : AttackEvent
    {
        public bool useEntityMaxFallSpeed;
        public bool useEntityGravity;
        public bool useEntityGravityScale;

        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityController controller, AttackEventVariables variables)
        {
            /*
            float percent = (float)frame / (float)endFrame;

            float gravity = controller.definition.stats.gravity;
            if (!useEntityGravity)
            {
                gravity = variables.curveVars[0].Evaluate(percent)
                    * variables.floatVars[0];
            }

            float gravityScale = controller.PhysicsManager.GravityScale;
            if (!useEntityGravityScale)
            {
                gravityScale = variables.curveVars[1].Evaluate(percent)
                    * variables.floatVars[1];
            }

            float maxFallSpeed = controller.definition.stats.maxFallSpeed;
            if (!useEntityMaxFallSpeed)
            {
                maxFallSpeed = variables.curveVars[2].Evaluate(percent)
                    * variables.floatVars[2];
            }

            controller.PhysicsManager.HandleGravity(maxFallSpeed, gravity, gravityScale);*/
            return false;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(AttackEventDefinition eventDefinition)
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

            if (!useEntityGravity)
            {
                eventDefinition.variables.floatVars[0] = EditorGUILayout.FloatField("Gravity Curve Multiplier",
                    eventDefinition.variables.floatVars[0]);
                eventDefinition.variables.curveVars[0] = EditorGUILayout.CurveField("Gravity Curve",
                    eventDefinition.variables.curveVars[0]);
            }
            if (!useEntityGravityScale)
            {
                eventDefinition.variables.floatVars[1] = EditorGUILayout.FloatField("Gravity Scale Curve Multiplier",
                    eventDefinition.variables.floatVars[1]);
                eventDefinition.variables.curveVars[1] = EditorGUILayout.CurveField("Gravity Scale Curve",
                    eventDefinition.variables.curveVars[1]);
            }
            if (!useEntityMaxFallSpeed)
            {
                eventDefinition.variables.floatVars[2] = EditorGUILayout.FloatField("Max Fall Speed Multiplier",
                    eventDefinition.variables.floatVars[2]);
                eventDefinition.variables.curveVars[2] = EditorGUILayout.CurveField("Max Fall Speed Curve",
                    eventDefinition.variables.curveVars[2]);
            }
        }
#endif
    }
}