using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityGroundBounce : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            PhysicsManager.forceGravity.y = GameManager.current.gameVariables.combat.groundBounceForce;
        }

        public override void OnUpdate()
        {

            PhysicsManager.ApplyMovementFriction(controller.definition.stats.hitstunFrictionXZ);
            //if (StateManager.CurrentStateFrame > 9)
            //{
            PhysicsManager.HandleGravity(controller.definition.stats.hitstunMaxFallSpeed,
                controller.definition.stats.hitstunGravity, 1.0f);
            //}

            StateManager.IncrementFrame();

            CheckInterrupt();
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
            /*if (controller.IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.FLINCH_AIR, StateManager.CurrentStateFrame);
                return true;
            }*/
            return false;
        }

        public override string GetName()
        {
            return "Ground Bounce";
        }
    }
}