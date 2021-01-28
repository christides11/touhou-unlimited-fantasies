using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CVertWallRun : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            PhysicsManager.forceMovement = Vector3.zero;
            PhysicsManager.forceGravity = Vector3.zero;

            ((CharacterManager)controller).currentAirDash = 0;
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            PhysicsManager.forceGravity = Vector3.up * ((CharacterStats)controller.EntityStats).wallRunVerticalSpeed;

            StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            if (((CharacterManager)controller).TryLedgeGrab())
            {
                return true;
            }
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).firstPress
                || PhysicsManager.DetectWall(out int v, true).collider == null)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_JUMP);
                return true;
            }

            if(StateManager.CurrentStateFrame > 30)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
            }
            return false;
        }
    }
}