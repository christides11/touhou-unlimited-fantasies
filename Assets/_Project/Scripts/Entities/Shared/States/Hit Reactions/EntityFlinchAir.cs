using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityFlinchAir : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            PhysicsManager.ApplyMovementFriction(controller.definition.stats.hitstunFrictionXZ);
            if (StateManager.CurrentStateFrame > 9)
            {
                PhysicsManager.HandleGravity(controller.definition.stats.hitstunMaxFallSpeed,
                    controller.definition.stats.hitstunGravity, 1.0f);
            }

            StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            if (CombatManager.HitStun == 0)
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
                if (((HitInfo)CombatManager.LastHitBy).groundBounces)
                {
                    StateManager.ChangeState((int)EntityStates.GROUND_BOUNCE);
                }
                else
                {
                    StateManager.ChangeState((int)EntityStates.FLINCH, StateManager.CurrentStateFrame, false);
                }
                return true;
            }
            return false;
        }

        public override string GetName()
        {
            return "Flinch (Air)";
        }
    }
}
