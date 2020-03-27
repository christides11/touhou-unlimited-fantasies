using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityJump : EntityState
    {

        public override void OnStart()
        {
            base.OnStart();
            controller.IsGrounded = false;
            controller.ForcesManager.forceGravity.y = controller.definition.stats.fullHopVelocity;
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                EntityStats es = controller.definition.stats;
                controller.ForcesManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration, es.airRotationSpeed);
            }
        }

        public override string GetName()
        {
            return "Jump";
        }
    }
}