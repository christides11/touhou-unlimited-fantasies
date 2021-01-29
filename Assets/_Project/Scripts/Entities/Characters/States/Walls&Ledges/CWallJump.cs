using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CWallJump : EntityWallJump
    {

        public override bool CheckInterrupt()
        {
            if (controller.StateManager.CurrentStateFrame > 3)
            {
                if (controller.CanAirJump())
                {
                    controller.StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                    return true;
                }
            }
            if (((CharacterManager)controller).CheckAirDash())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if (controller.StateManager.CurrentStateFrame >= controller.EntityStats.wallJumpTime)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}