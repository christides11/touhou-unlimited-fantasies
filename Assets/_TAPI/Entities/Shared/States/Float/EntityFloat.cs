using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityFloat : EntityState
    {

        public override void OnStart()
        {
            base.OnStart();
            controller.IsFloating = true;
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            controller.PhysicsManager.forceGravity.y = controller.InputManager.GetFloatDir(0)
                * controller.definition.stats.floatVerticalSpeed;

            if(controller.InputManager.GetMovement(0).magnitude <= InputConstants.movementMagnitude)
            {
                controller.PhysicsManager.ApplyMovementFriction(controller.definition.stats.floatFriction);
            }
            else
            {
                HandleMovement();
            }
        }

        protected virtual void HandleMovement()
        {
            Vector2 movement = controller.InputManager.GetMovement(0);
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);

            Vector3 velo = (translatedMovement * controller.definition.stats.floatAcceleration)
                + (translatedMovement.normalized * controller.definition.stats.floatBaseAccel);

            controller.PhysicsManager.forceMovement += velo;
            //Limit movement velocity.
            if (controller.PhysicsManager.forceMovement.magnitude > 
                controller.definition.stats.maxFloatSpeed * translatedMovement.magnitude)
            {
                controller.PhysicsManager.forceMovement = controller.PhysicsManager.forceMovement.normalized
                    * controller.definition.stats.maxFloatSpeed * translatedMovement.magnitude;
            }

            if (controller.LockedOn)
            {
                controller.RotateVisual(controller.LockonForward, controller.definition.stats.floatRotationSpeed);
            }
            else
            {
                controller.RotateVisual(controller.PhysicsManager.forceMovement, controller.definition.stats.floatRotationSpeed);
            }
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            controller.IsFloating = false;
        }

        public override string GetName()
        {
            return "Float";
        }
    }
}