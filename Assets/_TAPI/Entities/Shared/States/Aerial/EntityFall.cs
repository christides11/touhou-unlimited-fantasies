using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityFall : EntityState
    {

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                EntityStats es = controller.definition.stats;
                PhysicsManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration, es.airRotationSpeed);
                PhysicsManager.HandleGravity(es.gravity);
            }
        }
    }
}
