using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityJump : EntityState
    {

        public override void Initialize()
        {
            base.Initialize();
            controller.IsGrounded = false;

            Vector3 mVector = controller.GetMovementVector();
            mVector.y = 0;
            if (mVector.magnitude >= InputConstants.movementMagnitude)
            {
                PhysicsManager.forceMovement = mVector;
                PhysicsManager.forceMovement *= controller.EntityStats.jumpHorizontalMomentum;
            }

            // Transfer moving platform forces into actual force.
            Vector3 tempPhysicsMover = controller.cc.Motor.AttachedRigidbodyVelocity;
            PhysicsManager.forceGravity.y = tempPhysicsMover.y;
            tempPhysicsMover.y = 0;
            PhysicsManager.forceMovement += tempPhysicsMover;

            // Ignore negative gravity.
            if (PhysicsManager.forceGravity.y < 0)
            {
                PhysicsManager.forceGravity.y = 0;
            }

            // Add jump force.
            PhysicsManager.forceGravity.y += controller.fullHop ? controller.EntityStats.fullHopVelocity
                : controller.EntityStats.shortHopJumpVelocity;
        }

        public override void OnUpdate()
        {
            EntityStats es = controller.EntityStats;
            PhysicsManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration);
            PhysicsManager.HandleGravity();
            controller.RotateVisual(controller.GetMovementVector(0), es.airRotationSpeed);

            CheckInterrupt();
        }

        public override string GetName()
        {
            return "Jump";
        }
    }
}