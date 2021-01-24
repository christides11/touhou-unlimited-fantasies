using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CDash : EntityDash
    {

        public override bool CheckInterrupt()
        {
            if (controller.TryAttack())
            {
                return true;
            }
            if (InputManager.GetButton((int)EntityInputs.Dash).firstPress
                && controller.StateManager.CurrentStateFrame >= 3)
            {
                StateManager.ChangeState((int)EntityStates.DASH);
                return true;
            }
            if (InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (!controller.IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (StateManager.CurrentStateFrame >= controller.definition.stats.dashTime)
            {
                StateManager.ChangeState((int)EntityStates.RUN);
                return true;
            }
            return false;
        }
    }
}