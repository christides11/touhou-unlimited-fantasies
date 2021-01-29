using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityWallBounce : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            PhysicsManager.forceGravity = Vector3.zero;
            PhysicsManager.forceMovement = Vector3.zero;
        }

        public override void OnUpdate()
        {
            EntityManager em = ((EntityManager)Manager);
            if (StateManager.CurrentStateFrame == 4)
            {
                Vector3 dir = em.lastWallHit.normal;
                dir.y = 0;


                PhysicsManager.forceMovement = dir * ((HitInfo)em.CombatManager.LastHitBy).wallBounceForce;
            }

            StateManager.IncrementFrame();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            if (StateManager.CurrentStateFrame > 6)
            {
                StateManager.ChangeState((int)EntityStates.TUMBLE);
                return true;
            }
            return false;
        }
    }
}