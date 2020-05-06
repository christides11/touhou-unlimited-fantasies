using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class PFall : EntityFall
    {

        public override bool CheckInterrupt()
        {
            if (controller.CombatManager.CheckForAction())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
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
            if (controller.InputManager.GetButton(EntityInputs.Dash).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if (controller.IsGrounded)
            {
                if (controller.InputManager.GetMovement(0).magnitude > InputConstants.movementMagnitude)
                {
                    if (((CharacterController)controller).wasRunning)
                    {
                        controller.StateManager.ChangeState((int)EntityStates.RUN);
                    }
                    else
                    {
                        controller.StateManager.ChangeState((int)EntityStates.WALK);
                    }
                }
                else
                {
                    controller.StateManager.ChangeState((int)EntityStates.IDLE);
                }
                return true;
            }
            return false;
        }
    }
}
