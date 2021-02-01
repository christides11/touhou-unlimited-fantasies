using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityWallBounce : EntityState
    {
        Vector3 storedForceMovement;
        Vector3 storedForceGravity;
        public override void Initialize()
        {
            base.Initialize();
            storedForceMovement = PhysicsManager.forceMovement;
            storedForceGravity = PhysicsManager.forceGravity;
            PhysicsManager.forceGravity = Vector3.zero;
            PhysicsManager.forceMovement = Vector3.zero;
        }

        public override void OnUpdate()
        {
            EntityManager em = ((EntityManager)Manager);
            if (StateManager.CurrentStateFrame == 4)
            {
                PhysicsManager.forceMovement = -storedForceMovement.normalized * ((HitInfo)em.CombatManager.LastHitBy).wallBounceForce;
            }

            CheckInterrupt();
            StateManager.IncrementFrame();
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