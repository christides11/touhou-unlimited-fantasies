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
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            PhysicsManager.forceGravity = Vector3.up * ((CharacterStats)controller.definition.stats).wallRunVerticalSpeed;

            StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).firstPress
                || PhysicsManager.DetectWall(true).collider == null
                || StateManager.CurrentStateFrame > 30)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_JUMP);
                return true;
            }
            return false;
        }
    }
}