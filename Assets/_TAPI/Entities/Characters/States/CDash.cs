using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class CDash : EntityDash
    {

        public override bool CheckInterrupt()
        {
            EntityInput ei = controller.InputManager;
            if (controller.CombatManager.CheckForAction())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (ei.GetButton(EntityInputs.Dash).firstPress
                && controller.StateManager.CurrentStateFrame >= 3)
            {
                controller.StateManager.ChangeState((int)EntityStates.DASH);
                return true;
            }
            if (ei.GetButton(EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (!controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (controller.StateManager.CurrentStateFrame >= controller.definition.stats.dashTime)
            {
                controller.StateManager.ChangeState((int)EntityStates.RUN);
                return true;
            }
            return false;
        }
    }
}