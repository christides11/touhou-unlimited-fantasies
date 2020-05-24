using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityDash : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            Vector2 movement = controller.InputManager.GetAxis2D((int)EntityInputs.Movement).normalized;
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement *= controller.definition.stats.dashSpeed;

            controller.PhysicsManager.forceMovement = translatedMovement;
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