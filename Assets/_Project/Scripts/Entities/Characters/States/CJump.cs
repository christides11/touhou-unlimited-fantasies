using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CJump : EntityJump
    {

        public override bool CheckInterrupt()
        {
            if (CombatManager.TryAttack())
            {
                StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (controller.TryEnemyStep())
            {
                return true;
            }
            if (controller.CanAirJump())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            if (controller.TryFloat())
            {
                return true;
            }
            if (((CharacterManager)controller).CheckAirDash())
            {
                StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if(PhysicsManager.forceGravity.y <= 0)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}
