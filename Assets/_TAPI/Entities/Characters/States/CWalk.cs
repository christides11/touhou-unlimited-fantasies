using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class CWalk : EntityWalk
    {
        public override bool CheckInterrupt()
        {
            EntityInputManager ei = controller.InputManager;
            if (controller.CombatManager.TryAttack())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (ei.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (!controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (ei.GetButton((int)EntityInputs.Dash).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.DASH);
                return true;
            }
            if (ei.GetAxis2D((int)EntityInputs.Movement).magnitude <= InputConstants.movementMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }
    }
}
