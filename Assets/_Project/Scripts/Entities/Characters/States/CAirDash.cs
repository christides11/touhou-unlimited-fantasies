using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CAirDash : CharacterState
    {

        public override void Initialize()
        {
            base.Initialize();
            CharacterStats cs = ((CharacterStats)controller.definition.stats);

            ((CharacterManager)controller).currentAirDash++;
            PhysicsManager.forceGravity *= cs.airDashGravityInitMulti;
            Vector2 movement = controller.InputManager.GetAxis2D((int)EntityInputs.Movement);
            if(movement.magnitude < InputConstants.movementMagnitude)
            {
                Vector3 v = controller.lookHandler.LookTransform().InverseTransformDirection(controller.GetVisualBasedDirection(Vector3.forward));
                movement.x = v.x;
                movement.y = v.z;
            }
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement *= cs.airDashVelo;

            PhysicsManager.forceMovement = translatedMovement;
            controller.RotateVisual(translatedMovement, 100);
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                CharacterStats cs = ((CharacterStats)controller.definition.stats);
                if (controller.StateManager.CurrentStateFrame > cs.airDashHoldVelo)
                {
                    PhysicsManager.ApplyMovementFriction(cs.airDashFriction);
                }
                PhysicsManager.ApplyGravityFriction(cs.airDashGravityFriction);
                controller.StateManager.IncrementFrame();
            }
        }

        public override bool CheckInterrupt()
        {
            if (controller.StateManager.CurrentStateFrame >= 3)
            {
                if (controller.CombatManager.TryAttack())
                {
                    controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                    return true;
                }
            }
            if (((CharacterManager)controller).TryWallRun())
            {
                return true;
            }
            if (Mathf.Abs(controller.InputManager.GetAxis((int)EntityInputs.Float)) > InputConstants.floatMagnitude)
            {
                StateManager.ChangeState((int)EntityStates.FLOAT);
                return true;
            }
            if (controller.TryEnemyStep())
            {
                return true;
            }
            if (controller.CanAirJump())
            {
                StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            if (StateManager.CurrentStateFrame >= ((CharacterStats)controller.definition.stats).airDashLength)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }

        public override string GetName()
        {
            return "Air Dash";
        }
    }
}
