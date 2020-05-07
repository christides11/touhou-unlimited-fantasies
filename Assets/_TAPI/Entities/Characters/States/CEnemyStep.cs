using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class CEnemyStep : EntityState
    {
        Vector3 inertiaBackup;
        public override void OnStart()
        {
            base.OnStart();
            inertiaBackup = PhysicsManager.forceInertia;
            PhysicsManager.forceInertia = Vector3.zero;
            PhysicsManager.forceMovement = Vector3.zero;
            PhysicsManager.forceGravity = Vector3.zero;
            controller.ResetAirActions();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            if (CombatManager.CheckForAction())
            {
                PhysicsManager.forceInertia = inertiaBackup;
                StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if(StateManager.CurrentStateFrame >= 5)
            {
                StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            return false;
        }
    }
}