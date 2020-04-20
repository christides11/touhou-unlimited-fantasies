using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityFlinch : EntityState
    {

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            PhysicsManager.ApplyMovementFriction(controller.definition.stats.hitstunFriction);
        }

        public override bool CheckInterrupt()
        {
            if (CombatManager.hitStun == 0)
            {
                if (controller.IsGrounded)
                {
                    StateManager.ChangeState((int)EntityStates.IDLE);
                    return true;
                }
                else
                {
                    StateManager.ChangeState((int)EntityStates.FALL);
                    return true;
                }
            }
            if (!controller.IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.FLINCH_AIR, StateManager.CurrentStateFrame);
                return true;
            }
            return false;
        }

        public override string GetName()
        {
            return "Flinch";
        }
    }
}
