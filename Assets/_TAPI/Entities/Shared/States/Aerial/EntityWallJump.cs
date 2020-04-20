using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityWallJump : EntityState
    {
        public override void OnStart()
        {
            controller.IsGrounded = false;
            controller.PhysicsManager.forceGravity.y = controller.definition.stats.wallJumpYVelo;
            Vector3 translatedMovement = controller.PhysicsManager.wallRayHit.normal.normalized 
                * controller.definition.stats.wallJumpHVelo;
            controller.PhysicsManager.forceMovement = translatedMovement;
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                EntityStats es = controller.definition.stats;
                PhysicsManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration, es.airRotationSpeed);
                PhysicsManager.HandleGravity(es.gravity);
                StateManager.IncrementFrame();
            }
        }
    }
}