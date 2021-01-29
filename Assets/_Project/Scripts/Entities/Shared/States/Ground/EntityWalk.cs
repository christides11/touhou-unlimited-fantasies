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

            Vector3 velo = (translatedMovement * controller.EntityStats.walkAcceleration)
                + (translatedMovement.normalized * controller.EntityStats.walkBaseAccel);

            PhysicsManager.forceMovement += velo;
            //Limit movement velocity.
            if(PhysicsManager.forceMovement.magnitude > 
                controller.EntityStats.maxWalkSpeed * translatedMovement.magnitude)
            {
                PhysicsManager.forceMovement = PhysicsManager.forceMovement.normalized
                    * controller.EntityStats.maxWalkSpeed * translatedMovement.magnitude;
            }

            if (controller.LockedOn)
            {
                controller.RotateVisual(controller.LockonForward, controller.EntityStats.walkRotationSpeed);
            }
            else
            {
                controller.RotateVisual(translatedMovement, controller.EntityStats.walkRotationSpeed);
            }
        }

        public override string GetName()
        {
            return "Walk";
        }
    }
}