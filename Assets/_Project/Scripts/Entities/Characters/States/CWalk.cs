using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CWalk : EntityWalk
    {
        public override bool CheckInterrupt()
        {
            if (controller.CombatManager.TryAttack())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (!controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (InputManager.GetButton((int)EntityInputs.Dash).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.DASH);
                return true;
            }
            if (InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude <= InputConstants.movementMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }
    }
}
