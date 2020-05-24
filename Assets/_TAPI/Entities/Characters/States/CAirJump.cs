using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class CAirJump : EntityAirJump
    {
        public override bool CheckInterrupt()
        {
            if (controller.CombatManager.TryAttack())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
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
            if (Mathf.Abs(controller.InputManager.GetAxis((int)EntityInputs.Float)) > InputConstants.floatMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.FLOAT);
                return true;
            }
            if (((CharacterController)controller).CheckAirDash())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if (controller.PhysicsManager.forceGravity.y <= 0)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}