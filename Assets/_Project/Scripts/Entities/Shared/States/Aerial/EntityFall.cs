﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityFall : EntityState
    {
        public override string GetName()
        {
            return "Fall";
        }

        public override void OnUpdate()
        {
            EntityStats es = controller.EntityStats;
            PhysicsManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration);
            PhysicsManager.HandleGravity(es.gravity);
            controller.RotateVisual(controller.GetMovementVector(0), es.airRotationSpeed);

            CheckInterrupt();
        }
    }
}
