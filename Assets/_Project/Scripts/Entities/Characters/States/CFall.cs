using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CFall : EntityFall
    {

        public override bool CheckInterrupt()
        {
            if (controller.CombatManager.TryAttack())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (((CharacterManager)controller).TryLedgeGrab())
            {
                return true;
            }
            if (controller.TryFloat())
            {
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
            if (((CharacterManager)controller).CheckAirDash())
            {
                StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if (controller.IsGrounded)
            {
                if (controller.InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude > InputConstants.movementMagnitude)
                {
                    if (((CharacterManager)controller).wasRunning)
                    {
                        StateManager.ChangeState((int)EntityStates.RUN);
                    }
                    else
                    {
                        StateManager.ChangeState((int)EntityStates.WALK);
                    }
                }
                else
                {
                    StateManager.ChangeState((int)EntityStates.IDLE);
                }
                return true;
            }
            return false;
        }
    }
}
