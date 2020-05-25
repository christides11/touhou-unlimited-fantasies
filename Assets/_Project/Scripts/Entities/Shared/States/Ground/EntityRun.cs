using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityRun : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            controller.ResetAirActions();
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                Vector2 movement = controller.InputManager.GetAxis2D((int)EntityInputs.Movement);
                Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
                translatedMovement *= controller.definition.stats.runAcceleration;

                controller.PhysicsManager.forceMovement += translatedMovement;
                //Limit movement velocity.
                if (controller.PhysicsManager.forceMovement.magnitude > controller.definition.stats.maxRunSpeed * movement.magnitude)
                {
                    controller.PhysicsManager.forceMovement = controller.PhysicsManager.forceMovement.normalized
                        * controller.definition.stats.maxRunSpeed * movement.magnitude;
                }

                controller.RotateVisual(translatedMovement, controller.definition.stats.runRotationSpeed);
            }
        }

        public override string GetName()
        {
            return "Run";
        }
    }
}