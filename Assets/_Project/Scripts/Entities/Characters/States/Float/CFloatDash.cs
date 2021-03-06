﻿using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Characters;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CFloatDash : EntityState
    {
        private CharacterStats Stats { get { return (CharacterStats)controller.EntityStats; } }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                Vector3 totalMovement = PhysicsManager.forceMovement + PhysicsManager.forceGravity;

                Vector2 movement = controller.InputManager.GetAxis2D((int)EntityInputs.Movement);
                Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
                translatedMovement *= Stats.floatDashAcceleration;

                translatedMovement.y = controller.InputManager.GetAxis((int)EntityInputs.Float)
                    * Stats.floatDashAcceleration;

                totalMovement += translatedMovement;
                //Limit movement velocity.
                if (totalMovement.magnitude > Stats.floatDashMaxSpeed * translatedMovement.magnitude)
                {
                    totalMovement = totalMovement.normalized
                        * Stats.floatDashMaxSpeed * translatedMovement.magnitude;
                }

                // Set movement values.
                PhysicsManager.forceMovement.x = totalMovement.x;
                PhysicsManager.forceMovement.z = totalMovement.z;
                PhysicsManager.forceGravity.y = totalMovement.y;
            }
        }

        public override bool CheckInterrupt()
        {
            if (controller.InputManager.GetButton((int)EntityInputs.Dash).firstPress)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.FLOAT_DODGE);
                return true;
            }
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (controller.InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude <= InputConstants.movementMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.FLOAT);
                return true;
            }
            if (controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.RUN);
                return true;
            }
            return false;
        }
    }
}
