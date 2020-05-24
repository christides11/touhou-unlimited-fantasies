using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class CAttack : EntityAttack
    {

        public override bool CheckInterrupt()
        {
            if (CombatManager.TryAttack())
            {
                StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }

            if(controller.StateManager.CurrentStateFrame >
                controller.CombatManager.CurrentAttack.attackDefinition.length)
            {
                if (CombatManager.WasFloating)
                {
                    CombatManager.WasFloating = false;
                    StateManager.ChangeState((int)EntityStates.FLOAT);
                }
                else if(controller.IsGrounded)
                {
                    StateManager.ChangeState((int)EntityStates.IDLE);
                }
                else
                {
                    StateManager.ChangeState((int)EntityStates.FALL);
                }
                controller.CombatManager.Cleanup();
                return true;
            }
            return false;
        }
    }
}