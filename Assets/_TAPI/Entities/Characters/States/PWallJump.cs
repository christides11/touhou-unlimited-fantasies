using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class PWallJump : EntityWallJump
    {

        public override bool CheckInterrupt()
        {
            if (((CharacterController)controller).CheckAirDash())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if (controller.StateManager.CurrentStateFrame >= controller.definition.stats.wallJumpTime)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}