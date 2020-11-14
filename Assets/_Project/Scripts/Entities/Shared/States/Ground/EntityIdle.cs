﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityIdle : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            controller.ResetAirActions();
            controller.PhysicsManager.forceGravity = Vector3.zero;
        }

        public override void OnUpdate()
        {
            controller.PhysicsManager.ApplyMovementFriction();
            if(controller.LockedOn)
            {
                controller.RotateVisual(controller.LockonForward, 10);
            }
            CheckInterrupt();
        }

        public override string GetName()
        {
            return "Idle";
        }
    }
}