using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CLedgeJump : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            CharacterStats cs = (CharacterStats)controller.definition.stats;
            PhysicsManager.forceGravity.y = cs.ledgeJumpYForce;

            Vector3 cancelForce = -PhysicsManager.forceMovement;
            // Neutral input, just jump straight up.
            if(controller.GetMovementVector(0).magnitude < InputConstants.movementMagnitude)
            {
                PhysicsManager.forceMovement += cancelForce;
            }
            else
            {
                PhysicsManager.forceMovement += cs.ledgeJumpMoveForce * controller.visualTransform.forward;
            }
        }

        public override void OnUpdate()
        {
            CharacterStats cs = (CharacterStats)controller.definition.stats;

            //PhysicsManager.ApplyMovement(cs.airAcceleration, cs.ledgeJumpMaxSpeed, cs.airDeceleration);
            PhysicsManager.HandleGravity();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            if (controller.IsGrounded)
            {
                if (controller.InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude > InputConstants.movementMagnitude)
                {
                    if (((CharacterManager)controller).wasRunning)
                    {
                        StateManager.ChangeState((int)EntityStates.RUN);
                    }
                    else
                    {
                        StateManager.ChangeState((int)EntityStates.WALK);
                    }
                }
                else
                {
                    StateManager.ChangeState((int)EntityStates.IDLE);
                }
                return true;
            }
            return false;
        }

        public override string GetName()
        {
            return "Ledge Jump";
        }
    }
}