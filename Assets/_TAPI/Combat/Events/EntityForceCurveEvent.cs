using System.Collections;
using System.Collections.Generic;
using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "EntityFrictionEvent", menuName = "Attack Events/Force Curve")]
    public class EntityForceCurveEvent : AttackEvent
    {
        public bool xForce;
        public bool yForce;
        public bool zForce;

        public override void Evaluate(uint frame, uint endFrame, EntityAttack attackState, EntityController controller, 
            AttackEventVariables variables)
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
            if(variables.intVars[0] == 0)
            {
                if (yForce)
                {
                    controller.ForcesManager.forceGravity.y = tempY;
                }
                if (xForce || zForce)
                {
                    f.y = 0;
                    controller.ForcesManager.forceMovement = f;
                }
            }
            else
            {
                if (yForce)
                {
                    controller.ForcesManager.forceGravity.y += f.y;
                }
                if (xForce || zForce)
                {
                    f.y = 0;
                    controller.ForcesManager.forceMovement += f;
                }
            }
        }
    }
}