using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityFlinchAir : EntityState
    {
        public override void OnStart()
        {
            base.OnStart();
            ForcesManager.ApplyGravity = false;
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            ForcesManager.ApplyMovementFriction(controller.definition.stats.hitstunFriction);
            if (ForcesManager.forceGravity.y > 0.05f)
            {
                ForcesManager.ApplyGravityFriction(controller.definition.stats.hitstunFriction);
            }
            else
            {
                ForcesManager.HandleGravity(controller.definition.stats.maxFallSpeed, controller.definition.stats.hitstunGravity);
            }
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
            if (controller.IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.FLINCH, StateManager.CurrentStateFrame);
                return true;
            }
            return false;
        }

        public override void OnInterrupted()
        {
            ForcesManager.ApplyGravity = true;
        }

        public override string GetName()
        {
            return "Flinch (Air)";
        }
    }
}
