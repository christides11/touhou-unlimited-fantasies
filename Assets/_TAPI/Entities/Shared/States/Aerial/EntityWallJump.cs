using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityWallJump : EntityState
    {
        public override string GetName()
        {
            return "Wall Jump";
        }
        public override void OnStart()
        {
            controller.IsGrounded = false;

            Vector3 moveDir = controller.GetMovementVector();
            Vector3 wallNormalForward = controller.PhysicsManager.wallRayHit.normal.normalized;

            Vector3 translatedMovement = Vector3.zero;
            if(Vector3.Dot(wallNormalForward, moveDir.normalized) >= controller.definition.stats.wallJumpMinAngle)
            {
                translatedMovement = moveDir * controller.definition.stats.wallJumpHVelo;
            }
            else
            {
                translatedMovement = wallNormalForward * controller.definition.stats.wallJumpHVelo;
            }
            
            controller.PhysicsManager.forceGravity.y = controller.definition.stats.wallJumpYVelo;
            controller.PhysicsManager.forceMovement = translatedMovement;
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                EntityStats es = controller.definition.stats;
                PhysicsManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration);
                PhysicsManager.HandleGravity(es.gravity);
                controller.RotateVisual(controller.GetMovementVector(0), es.airRotationSpeed);
                StateManager.IncrementFrame();
            }
        }
    }
}