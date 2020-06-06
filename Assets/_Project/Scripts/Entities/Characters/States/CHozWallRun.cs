using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CHozWallRun : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            PhysicsManager.forceMovement = Vector3.zero;
            PhysicsManager.forceGravity = Vector3.zero;

            float v = Vector3.SignedAngle(controller.visualTransform.forward,
                -((CharacterController)controller).lastWallHit.normal,
                Vector3.up);

            Vector3 c = -Vector3.Cross(Vector3.up, ((CharacterController)controller).lastWallHit.normal);
            controller.visualTransform.rotation = Quaternion.LookRotation((v <= 0 ? 1 : -1) * c, Vector3.up);

            controller.transform.position = ((CharacterController)controller).lastWallHit.point
                + (((CharacterController)controller).lastWallHit.normal) * (controller.GetSize().x / 2.0f);
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            PhysicsManager.forceMovement = controller.visualTransform.forward * ((CharacterStats)controller.definition.stats).wallRunHorizontalSpeed;

            StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_JUMP);
                return true;
            }

            if (StateManager.CurrentStateFrame > 40
                || PhysicsManager.DetectWall(true).collider == null)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}