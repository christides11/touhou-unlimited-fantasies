using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityAirJump : EntityState
    {
        public override void OnStart()
        {
            base.OnStart();
            controller.currentAirJump++;
            controller.IsGrounded = false;
            controller.PhysicsManager.forceGravity.y = controller.definition.stats.airJumpVelocity;

            Vector2 movement = controller.InputManager.GetMovement(0);
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement *= controller.definition.stats.airJumpHorizontalVelo;

            controller.PhysicsManager.forceMovement *= controller.definition.stats.airJumpCarriedMomentum;
            controller.PhysicsManager.forceMovement += translatedMovement;
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                EntityStats es = controller.definition.stats;
                PhysicsManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration);
                PhysicsManager.HandleGravity(es.gravity);
                controller.RotateVisual(controller.GetMovementVector(0), es.airRotationSpeed);
            }
        }

        public override string GetName()
        {
            return "AirJump";
        }
    }
}