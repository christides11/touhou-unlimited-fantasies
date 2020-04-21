using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityDash : EntityState
    {
        public override void OnStart()
        {
            base.OnStart();
            Vector2 movement = controller.InputManager.GetMovement(0).normalized;
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