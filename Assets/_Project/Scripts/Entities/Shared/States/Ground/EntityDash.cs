﻿using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityDash : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            Vector2 movement = controller.InputManager.GetAxis2D((int)EntityInputs.Movement).normalized;
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement *= controller.EntityStats.dashSpeed;

            PhysicsManager.forceMovement = translatedMovement;
            controller.RotateVisual(translatedMovement, 100);
            controller.ResetAirActions();
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                controller.StateManager.IncrementFrame();
            }
        }

        public override string GetName()
        {
            return "Dash";
        }
    }
}