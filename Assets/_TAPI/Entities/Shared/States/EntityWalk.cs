using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityWalk : EntityState
    {
        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                Vector2 movement = controller.InputManager.GetMovement(0);
                Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
                translatedMovement.y = 0;

                Vector3 velo = (translatedMovement * controller.definition.stats.walkAcceleration)
                    + (translatedMovement.normalized * controller.definition.stats.walkBaseAccel);

                controller.ForcesManager.forceMovement += velo;
                //Limit movement velocity.
                if(controller.ForcesManager.forceMovement.magnitude > 
                    controller.definition.stats.maxWalkSpeed * translatedMovement.magnitude)
                {
                    controller.ForcesManager.forceMovement = controller.ForcesManager.forceMovement.normalized
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
        }

        public override string GetName()
        {
            return "Walk";
        }
    }
}