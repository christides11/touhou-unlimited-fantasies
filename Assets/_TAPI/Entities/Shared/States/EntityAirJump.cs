﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityAirJump : EntityState
    {
        public override void OnStart()
        {
            base.OnStart();
            controller.IsGrounded = false;
            controller.ForcesManager.forceGravity.y = controller.definition.stats.airJumpVelocity;

            Vector2 movement = controller.InputManager.GetMovement(0);
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement *= controller.definition.stats.airJumpHorizontalVelo;

            controller.ForcesManager.forceMovement = (controller.ForcesManager.forceMovement 
                * controller.definition.stats.airJumpCarriedMomentum) + translatedMovement;
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
            return "AirJump";
        }
    }
}