﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityTumble : EntityState
    {
        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            PhysicsManager.ApplyMovementFriction(controller.definition.stats.hitstunFriction);
            if (PhysicsManager.forceGravity.y > 0.05f)
            {
                PhysicsManager.ApplyGravityFriction(controller.definition.stats.hitstunFriction);
            }
            else
            {
                PhysicsManager.HandleGravity(controller.definition.stats.maxFallSpeed, controller.definition.stats.hitstunGravity);
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

        public override string GetName()
        {
            return "Tumble";
        }
    }
}