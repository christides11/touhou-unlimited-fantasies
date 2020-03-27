using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class PRun : EntityRun
    {
        public override void OnStart()
        {
            base.OnStart();
            ((CharacterController)controller).wasRunning = true;
        }

        public override bool CheckInterrupt()
        {
            EntityInput ei = controller.InputManager;
            if (controller.CombatManager.CheckForAction())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (ei.GetButton(EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (!controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (ei.GetMovement(0).magnitude <= InputConstants.movementMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
        }
    }
}
