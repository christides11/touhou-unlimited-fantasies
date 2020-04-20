using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Characters;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class PFloatDodge : EntityState
    {
        private CharacterStats Stats { get { return (CharacterStats)controller.definition.stats; } }

        public override void OnStart()
        {
            base.OnStart();
            controller.PhysicsManager.forceGravity.y = controller.InputManager.GetFloatDir(0)
                * Stats.floatDodgeVelo;

            Vector2 movement = controller.InputManager.GetMovement(0);
            Vector3 translatedMovement = Vector3.zero;
            if (movement.magnitude <= InputConstants.movementMagnitude)
            {
                movement = controller.PhysicsManager.forceGravity.y == 0 ? Vector2.zero
                    : Vector2.up;
            }
            else
            {
                movement.Normalize();
                translatedMovement = controller.GetMovementVector(movement.x, movement.y);
                translatedMovement *= Stats.floatDodgeVelo;
            }
            controller.PhysicsManager.forceMovement = translatedMovement;
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                if(controller.StateManager.CurrentStateFrame > Stats.floatDodgeHoldVelo)
                {
                    controller.PhysicsManager.ApplyMovementFriction(Stats.floatDodgeFriction);
                    controller.PhysicsManager.ApplyGravityFriction(Stats.floatDodgeFriction);
                }

                controller.StateManager.IncrementFrame();
            }
        }

        public override bool CheckInterrupt()
        {
            if (controller.IsGrounded)
            {
                if (controller.InputManager.GetMovement().magnitude > InputConstants.movementMagnitude)
                {
                    controller.StateManager.ChangeState((int)EntityStates.RUN);
                }
                else
                {
                    controller.StateManager.ChangeState((int)EntityStates.IDLE);
                }
                return true;
            }
            if(controller.StateManager.CurrentStateFrame > Stats.floatDodgeLength)
            {
                controller.StateManager.ChangeState((int)EntityStates.FLOAT);
                return true;
            }
            if(controller.StateManager.CurrentStateFrame > Stats.floatDodgeHoldVelo)
            {
                if (controller.InputManager.GetButton(EntityInputs.Dash).firstPress)
                {
                    controller.StateManager.ChangeState((int)BaseCharacterStates.FLOAT_DODGE);
                    return true;
                }else if (controller.InputManager.GetButton(EntityInputs.Dash).isDown)
                {
                    controller.StateManager.ChangeState((int)BaseCharacterStates.FLOAT_DASH);
                    return true;
                }
            }
            return false;
        }

        public override string GetName()
        {
            return "Float Dash";
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
        }
    }
}
