using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CIdle : EntityIdle
    {
        public override void Initialize()
        {
            base.Initialize();
            ((CharacterManager)controller).wasRunning = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (controller.InputManager.GetButton((int)EntityInputs.Dash).firstPress)
            {
                ((CharacterManager)controller).hoverMode = !((CharacterManager)controller).hoverMode;
            }
        }

        public override bool CheckInterrupt()
        {
            if (controller.TryAttack())
            {
                return true;
            }
            if (InputManager.GetAxis((int)EntityInputs.Float) < -0.5f)
            {
                StateManager.ChangeState((int)BaseCharacterStates.SLIDE);
                return true;
            }
            if (InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (!controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude > InputConstants.movementMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.WALK);
                return true;
            }
            return false;
        }
    }
}