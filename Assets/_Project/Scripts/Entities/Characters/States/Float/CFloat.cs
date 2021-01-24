using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CFloat : EntityFloat
    {
        public override bool CheckInterrupt()
        {
            if (controller.TryAttack())
            {
                return true;
            }
            if (controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (controller.InputManager.GetButton((int)EntityInputs.Dash).firstPress)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.FLOAT_DODGE);
                return true;
            }
            return false;
        }
    }
}