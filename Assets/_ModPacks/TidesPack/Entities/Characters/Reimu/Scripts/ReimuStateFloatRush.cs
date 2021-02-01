using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities;
using TUF.Entities.Characters;
using UnityEngine;

namespace TidesPack.Characters.Reimu
{
    public class ReimuStateFloatRush : CharacterState
    {
        public override void OnUpdate()
        {
            if (StateManager.CurrentStateFrame == 3)
            {
                ReimuStats rStats = (ReimuStats)controller.EntityStats;

                Vector3 targetDirection = controller.visualTransform.forward;
                if(controller.LockonTarget != null)
                {
                    targetDirection = controller.LockonTarget.transform.position - controller.transform.position;
                }
                targetDirection = targetDirection.normalized * rStats.floatRushSpeed;

                EntityPhysicsManager epm = ((EntityPhysicsManager)controller.PhysicsManager);
                epm.forceGravity.y = targetDirection.y;
                targetDirection.y = 0;
                epm.forceMovement = targetDirection;
            }

            CheckInterrupt();
            StateManager.IncrementFrame();
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            controller.CombatManager.Cleanup();
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
            ReimuStats rStats = (ReimuStats)controller.EntityStats;
            if (StateManager.CurrentStateFrame >= rStats.floatRushLength)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}