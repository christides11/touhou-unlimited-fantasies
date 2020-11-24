﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityJump : EntityState
    {

        public override void Initialize()
        {
            base.Initialize();
            controller.IsGrounded = false;
            PhysicsManager.forceGravity.y = controller.fullHop ? controller.definition.stats.fullHopVelocity
                : controller.definition.stats.shortHopJumpVelocity;
        }

        public override void OnUpdate()
        {
            EntityStats es = controller.definition.stats;
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