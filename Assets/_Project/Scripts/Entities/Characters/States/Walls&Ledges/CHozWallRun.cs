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
                -((CharacterManager)controller).lastWallHit.normal,
                Vector3.up);

            Vector3 c = -Vector3.Cross(Vector3.up, ((CharacterManager)controller).lastWallHit.normal);
            controller.visualTransform.rotation = Quaternion.LookRotation((v <= 0 ? 1 : -1) * c, Vector3.up);

            controller.transform.position = ((CharacterManager)controller).lastWallHit.point
                + (((CharacterManager)controller).lastWallHit.normal) * (controller.GetSize().x / 2.0f);

            ((CharacterManager)controller).currentAirDash = 0;
        }

        public override void OnUpdate()
        {
            UpdatePosition();

            PhysicsManager.forceMovement = 
                ((controller.visual.transform.forward
                * ((CharacterStats)controller.EntityStats).wallRunHorizontalSpeed
                * ((CharacterManager)controller).wallRunHozMultiplier))
                + 
                (controller.visual.transform.right * ((CharacterManager)controller).wallSide
                * ((CharacterStats)controller.EntityStats).wallRunHorizontalSpeed
                * ((CharacterManager)controller).wallRunHozMultiplier);

            StateManager.IncrementFrame();

            CheckInterrupt();
        }

        void UpdatePosition()
        {
            RaycastHit rHit;
            Physics.Raycast(controller.transform.position + new Vector3(0, 1, 0),
                 (controller.visualTransform.right * ((CharacterManager)controller).wallSide).normalized, 
                out rHit, PhysicsManager.wallCheckDistance * 1.25f, controller.GroundedLayerMask);

            Debug.DrawRay(controller.transform.position + new Vector3(0, 1, 0),
                controller.visualTransform.right * ((CharacterManager)controller).wallSide * PhysicsManager.wallCheckDistance);

            if(rHit.collider == null)
            {
                return;
            }

            float v = Vector3.SignedAngle(controller.visualTransform.forward,
            -(rHit.normal),
            Vector3.up);

            Vector3 c = -Vector3.Cross(Vector3.up, rHit.normal);
            controller.visualTransform.rotation = Quaternion.LookRotation((v <= 0 ? 1 : -1) * c, Vector3.up);

            controller.transform.position = rHit.point
                + ((rHit.normal.normalized) * (controller.GetSize().x / 2.0f));
        }

        public override bool CheckInterrupt()
        {
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_JUMP);
                return true;
            }

            if (StateManager.CurrentStateFrame > ((CharacterStats)controller.EntityStats).wallRunTime
                || PhysicsManager.DetectWall(out int v, true).collider == null)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            ((CharacterManager)controller).wallRunHozMultiplier 
                += ((CharacterStats)controller.EntityStats).wallRunHorizontalChainMulti;
        }
    }
}