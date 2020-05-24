using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class CIdle : EntityIdle
    {
        public override void Initialize()
        {
            base.Initialize();
            ((CharacterController)controller).wasRunning = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (controller.InputManager.GetButton((int)EntityInputs.Dash).firstPress)
            {
                ((CharacterController)controller).hoverMode = !((CharacterController)controller).hoverMode;
            }
        }

        public override bool CheckInterrupt()
        {
            EntityInputManager ei = controller.InputManager;
            if (controller.CombatManager.TryAttack())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (ei.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (!controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (ei.GetAxis2D((int)EntityInputs.Movement).magnitude > InputConstants.movementMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.WALK);
                return true;
            }
            return false;
        }
    }
}