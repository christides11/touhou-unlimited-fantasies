using System.Collections;
using System.Collections.Generic;
using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "EntityForceSetEvent", menuName = "Attack Events/Force Set")]
    public class EntityForceSetEvent : AttackEvent
    {
        public bool xzForce;
        public bool yForce;

        public override void Evaluate(uint frame, uint endFrame, EntityAttack attackState, EntityController controller,
    AttackEventVariables variables)
        {
            Vector3 f = Vector3.zero;
            if (xzForce)
            {
                f.x = variables.floatVars[0];
                f.z = variables.floatVars[1];
            }
            if (yForce)
            {
                f.y = variables.floatVars[0];
            }

            f = controller.GetVisualBasedDirection(f);

            if (yForce)
            {
                controller.ForcesManager.forceGravity.y = f.y;
            }
            if(xzForce)
            {
                f.y = 0;
                controller.ForcesManager.forceMovement = f;
            }
        }
    }
}
