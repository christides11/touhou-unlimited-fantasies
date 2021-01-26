using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CCrouchJump : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            controller.IsGrounded = false;

            // Transfer moving platform forces into actual force.
            Vector3 tempPhysicsMover = controller.cc.Motor.AttachedRigidbodyVelocity;
            PhysicsManager.forceGravity.y = tempPhysicsMover.y;
            tempPhysicsMover.y = 0;
            PhysicsManager.forceMovement += tempPhysicsMover;

            // Ignore negative gravity.
            if (PhysicsManager.forceGravity.y < 0)
            {
                PhysicsManager.forceGravity.y = 0;
            }

            // Add jump force.
            PhysicsManager.forceGravity.y += controller.definition.stats.crouchJumpVelocity;
        }

        public override void OnUpdate()
        {
            EntityStats es = controller.definition.stats;
            PhysicsManager.ApplyMovement(es.crouchJumpAcceleration, es.crouchJumpMaxAirSeed, es.crouchJumpDeceleration, false);
            PhysicsManager.HandleGravity();
            controller.RotateVisual(controller.GetMovementVector(0), es.airRotationSpeed);

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            if (controller.TryAttack())
            {
                return true;
            }
            if (((CharacterManager)controller).TryLedgeGrab())
            {
                return true;
            }
            if (((CharacterManager)controller).TryWallCling())
            {
                return true;
            }
            if (controller.TryEnemyStep())
            {
                return true;
            }
            if (controller.CanAirJump())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            if (controller.TryFloat())
            {
                return true;
            }
            if (((CharacterManager)controller).CheckAirDash())
            {
                StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
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
            return "Crouch Jump";
        }
    }
}