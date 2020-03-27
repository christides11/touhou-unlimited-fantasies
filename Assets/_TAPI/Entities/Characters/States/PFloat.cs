using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class PFloat : EntityFloat
    {
        public override bool CheckInterrupt()
        {
            if (controller.CombatManager.CheckForAction())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            if (controller.InputManager.GetButton(EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (controller.InputManager.GetButton(EntityInputs.Dash).firstPress)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.FLOAT_DODGE);
                return true;
            }
            return false;
        }
    }
}