using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityAirJump : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            controller.currentAirJump++;
            controller.IsGrounded = false;
            PhysicsManager.forceGravity.y = controller.EntityStats.airJumpVelocity;

            Vector2 movement = controller.InputManager.GetAxis2D((int)EntityInputs.Movement);
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement *= controller.EntityStats.airJumpHorizontalVelo;

            PhysicsManager.forceMovement *= controller.EntityStats.airJumpCarriedMomentum;
            PhysicsManager.forceMovement += translatedMovement;
        }

        public override void OnUpdate()
        {
            EntityStats es = controller.EntityStats;
            PhysicsManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration);
            PhysicsManager.HandleGravity(es.gravity);
            controller.RotateVisual(controller.GetMovementVector(0), es.airRotationSpeed);
            CheckInterrupt();
        }

        public override string GetName()
        {
            return "AirJump";
        }
    }
}