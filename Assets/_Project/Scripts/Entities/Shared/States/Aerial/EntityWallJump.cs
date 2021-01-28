using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityWallJump : EntityState
    {
        public override string GetName()
        {
            return "Wall Jump";
        }
        public override void Initialize()
        {
            controller.IsGrounded = false;

            Vector3 moveDir = controller.GetMovementVector();
            Vector3 wallNormalForward = PhysicsManager.wallRayHit.normal.normalized;

            Vector3 translatedMovement = Vector3.zero;
            if(moveDir.magnitude >= InputConstants.movementMagnitude
                || Vector3.Dot(wallNormalForward, moveDir.normalized) >= controller.EntityStats.wallJumpMinAngle)
            {
                translatedMovement = moveDir * controller.EntityStats.wallJumpHVelo;
            }
            else
            {
                translatedMovement = wallNormalForward * controller.EntityStats.wallJumpHVelo;
            }
            
            PhysicsManager.forceGravity.y = controller.EntityStats.wallJumpYVelo;
            PhysicsManager.forceMovement = translatedMovement;
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                EntityStats es = controller.EntityStats;
                PhysicsManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration);
                PhysicsManager.HandleGravity(es.gravity);
                controller.RotateVisual(controller.GetMovementVector(0), es.airRotationSpeed);
                StateManager.IncrementFrame();
            }
        }
    }
}