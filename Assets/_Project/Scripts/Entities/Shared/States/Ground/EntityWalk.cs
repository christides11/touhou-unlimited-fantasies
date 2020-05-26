using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityWalk : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            controller.ResetAirActions();
        }
        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            Vector2 movement = controller.InputManager.GetAxis2D((int)EntityInputs.Movement);
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement.y = 0;

            Vector3 velo = (translatedMovement * controller.definition.stats.walkAcceleration)
                + (translatedMovement.normalized * controller.definition.stats.walkBaseAccel);

            controller.PhysicsManager.forceMovement += velo;
            //Limit movement velocity.
            if(controller.PhysicsManager.forceMovement.magnitude > 
                controller.definition.stats.maxWalkSpeed * translatedMovement.magnitude)
            {
                controller.PhysicsManager.forceMovement = controller.PhysicsManager.forceMovement.normalized
                    * controller.definition.stats.maxWalkSpeed * translatedMovement.magnitude;
            }

            if (controller.LockedOn)
            {
                controller.RotateVisual(controller.LockonForward, controller.definition.stats.walkRotationSpeed);
            }
            else
            {
                controller.RotateVisual(translatedMovement, controller.definition.stats.walkRotationSpeed);
            }
        }

        public override string GetName()
        {
            return "Walk";
        }
    }
}