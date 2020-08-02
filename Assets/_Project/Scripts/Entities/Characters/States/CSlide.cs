﻿using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CSlide : CharacterState
    {
        public override void Initialize()
        {
            CharacterStats cs = ((CharacterStats)controller.definition.stats);

            base.Initialize();

            controller.PhysicsManager.forceMovement = controller.GetMovementVector(0).normalized * cs.slideInitialSpeed;
        }

        protected RaycastHit hit;
        protected Vector3 slopeParallel;
        protected float slopeAngle;
        protected Vector3 targetSpeed;
        public override void OnUpdate()
        {
            CharacterStats cs = ((CharacterStats)controller.definition.stats);

            GetSlopeInformation();

            Vector3 ConstForce = Vector3.zero;

            if (StateManager.CurrentStateFrame >= cs.slideHoldFrames)
            {
                if (slopeAngle >= 5.0f)
                {
                    ConstForce = slopeParallel;
                    ConstForce.y = 0;
                    ConstForce.Normalize();

                    targetSpeed = (cs.slideSlopeBaseSpeed + (slopeAngle * cs.slideSpeedPerAngle)) * ConstForce;
                    Debug.Log(targetSpeed.magnitude);
                }
                else
                {
                    targetSpeed = Vector3.zero;
                }

                controller.PhysicsManager.forceMovement = Vector3.Lerp(controller.PhysicsManager.forceMovement, targetSpeed,
                    Time.fixedDeltaTime * cs.slideForceTransitionTime);
            }

            PhysicsManager.HandleGravity();

            CheckInterrupt();

            StateManager.IncrementFrame();
        }

        private void GetSlopeInformation()
        {
            // Raycast with infinite distance to check the slope directly under the player no matter where they are
            Physics.Raycast(controller.transform.position, Vector3.down, out hit, Mathf.Infinity);

            // Saving the normal
            Vector3 n = hit.normal;

            // Crossing my normal with the player's up vector (if your player rotates I guess you can just use Vector3.up to create a vector parallel to the ground
            Vector3 groundParallel = Vector3.Cross(controller.transform.up, n);

            // Crossing the vector we made before with the initial normal gives us a vector that is parallel to the slope and always pointing down
            slopeParallel = Vector3.Cross(groundParallel, n);
            Debug.DrawRay(hit.point, slopeParallel * 10, Color.green);

            // Just the current angle we're standing on
            slopeAngle = Mathf.Round(Vector3.Angle(hit.normal, controller.transform.up));
        }

        public override bool CheckInterrupt()
        {
            if (InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                return true;
            }
            if (InputManager.GetAxis((int)EntityInputs.Float, 0) >= -0.5f
                && InputManager.GetAxis((int)EntityInputs.Float, 5) >= -0.5f)
            {
                StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }
    }
}