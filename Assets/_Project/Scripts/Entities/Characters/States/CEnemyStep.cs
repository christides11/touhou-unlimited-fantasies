using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CEnemyStep : EntityState
    {
        Vector3 forceBackup;
        public override void Initialize()
        {
            base.Initialize();
            forceBackup = controller.visualTransform.InverseTransformDirection(PhysicsManager.forceMovement);
            PhysicsManager.forceMovement = Vector3.zero;
            PhysicsManager.forceGravity = Vector3.zero;
            controller.ResetAirActions();
            controller.currentAirJump = -1;
            CombatManager.Reset();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Vector3 movement = controller.GetMovementVector();

            if (movement.magnitude > InputConstants.movementMagnitude)
            {
                controller.SetVisualRotation(controller.GetMovementVector());
            }
            StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            if (CombatManager.TryAttack())
            {
                PhysicsManager.forceMovement = controller.visualTransform.TransformDirection(forceBackup);
                StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if(StateManager.CurrentStateFrame >= 6)
            {
                StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            return false;
        }
    }
}