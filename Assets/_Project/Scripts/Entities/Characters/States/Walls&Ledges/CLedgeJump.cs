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
            CharacterStats cs = (CharacterStats)controller.EntityStats;
            PhysicsManager.forceGravity.y = cs.ledgeJumpYForce;

            PhysicsManager.forceMovement = PhysicsManager.forceMovement.magnitude * controller.visualTransform.forward;
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
            CharacterStats cs = (CharacterStats)controller.EntityStats;

            PhysicsManager.ApplyMovement(cs.ledgeJumpAccel, cs.ledgeJumpMaxSpeed, cs.airDeceleration, false);
            PhysicsManager.HandleGravity();

            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            if (CombatManager.TryAttack())
            {
                StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (((CharacterManager)controller).TryLedgeGrab())
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
            return "Ledge Jump";
        }
    }
}