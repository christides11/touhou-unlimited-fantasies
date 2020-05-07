using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class CAirDash : CharacterState
    {
        public override void OnStart()
        {
            base.OnStart();
            ((CharacterController)controller).currentAirDash++;
            controller.PhysicsManager.forceGravity = Vector3.zero;
            Vector2 movement = controller.InputManager.GetMovement(0);
            if(movement.magnitude < InputConstants.movementMagnitude)
            {
                Vector3 v = controller.lookTransform.InverseTransformDirection(controller.GetVisualBasedDirection(Vector3.forward));
                movement.x = v.x;
                movement.y = v.z;
            }
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement *= ((CharacterStats)controller.definition.stats).airDashVelo;

            controller.PhysicsManager.forceMovement = translatedMovement;
            controller.RotateVisual(translatedMovement, 100);
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                CharacterStats ps = ((CharacterStats)controller.definition.stats);
                if (controller.StateManager.CurrentStateFrame > ps.airDashHoldVelo)
                {
                    controller.PhysicsManager.ApplyMovementFriction(ps.airDashFriction);
                }
                controller.StateManager.IncrementFrame();
            }
        }

        public override bool CheckInterrupt()
        {
            if (controller.StateManager.CurrentStateFrame >= 3)
            {
                if (controller.CombatManager.CheckForAction())
                {
                    controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                    return true;
                }
            }
            RaycastHit rh = controller.PhysicsManager.DetectWall();
            if (rh.collider)
            {
                controller.PhysicsManager.currentWall = rh.transform.gameObject;
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_CLING);
                return true;
            }
            if (Mathf.Abs(controller.InputManager.GetFloatDir()) > InputConstants.floatMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.FLOAT);
                return true;
            }
            if (controller.EnemyStepCancel())
            {
                return true;
            }
            if (controller.CheckAirJump())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            if (controller.StateManager.CurrentStateFrame >= ((CharacterStats)controller.definition.stats).airDashLength)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
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
