using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityFloat : EntityState
    {
        public override string GetName()
        {
            return "Float";
        }

        public override void Initialize()
        {
            base.Initialize();
            controller.IsFloating = true;
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            PhysicsManager.forceGravity.y = controller.InputManager.GetAxis((int)EntityInputs.Float)
                * controller.definition.stats.floatVerticalSpeed;

            if(controller.InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude <= InputConstants.movementMagnitude)
            {
                PhysicsManager.ApplyMovementFriction(controller.definition.stats.floatFriction);
            }
            else
            {
                HandleMovement();
            }

            if (controller.LockedOn)
            {
                controller.RotateVisual(controller.LockonForward, controller.definition.stats.floatLockOnRotationSpeed);
            }
        }

        protected virtual void HandleMovement()
        {
            Vector2 movement = controller.InputManager.GetAxis2D((int)EntityInputs.Movement);
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);

            Vector3 velo = (translatedMovement * controller.definition.stats.floatAcceleration)
                + (translatedMovement.normalized * controller.definition.stats.floatBaseAccel);

            PhysicsManager.forceMovement += velo;
            //Limit movement velocity.
            if (PhysicsManager.forceMovement.magnitude > 
                controller.definition.stats.maxFloatSpeed * translatedMovement.magnitude)
            {
                PhysicsManager.forceMovement = PhysicsManager.forceMovement.normalized
                    * controller.definition.stats.maxFloatSpeed * translatedMovement.magnitude;
            }

            if (controller.LockedOn)
            {
                controller.RotateVisual(controller.LockonForward, controller.definition.stats.floatRotationSpeed);
            }
            else
            {
                controller.RotateVisual(PhysicsManager.forceMovement, controller.definition.stats.floatRotationSpeed);
            }
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            controller.IsFloating = false;
        }
    }
}