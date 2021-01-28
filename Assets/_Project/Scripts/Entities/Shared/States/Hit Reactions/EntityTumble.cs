using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityTumble : EntityState
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

            PhysicsManager.ApplyMovementFriction(controller.EntityStats.hitstunFrictionXZ);
            PhysicsManager.HandleGravity(controller.EntityStats.hitstunMaxFallSpeed,
                controller.EntityStats.hitstunGravity, 1.0f);
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
            if (!controller.IsGrounded && controller.FindWall())
            {
                StateManager.ChangeState((int)EntityStates.WALL_BOUNCE);
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