using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities;
using TAPI.Entities.Characters;
using UnityEngine;

namespace TidesPack.Characters.Reimu
{
    public class ReimuStateTeleport : CharacterState
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }

            if(StateManager.CurrentStateFrame == 10)
            {
                ReimuStats rStats = (ReimuStats)controller.definition.stats;

                Vector3 forwardDir = controller.visual.transform.forward;

                Vector3 endPosition = controller.transform.position 
                    + (controller.visual.transform.forward * rStats.teleportNoTargetForwardDist)
                    + (controller.visual.transform.up * rStats.teleportNoTargetUpDist);
                if (controller.LockedOn)
                {
                    if (controller.LockonTarget)
                    {
                        Vector3 towardSelf = controller.transform.position - controller.LockonTarget.transform.position;
                        towardSelf.y = 0;
                        endPosition = controller.LockonTarget.transform.position
                            + (towardSelf.normalized * rStats.teleportTargetForwardDist)
                            + (Vector3.up * rStats.teleportTargetUpDist);
                        forwardDir = towardSelf * -1f;
                    }
                }

                controller.cc.Motor.SetPosition(endPosition);
                controller.PhysicsManager.forceGravity.y = rStats.teleportUpwardForce;
                controller.PhysicsManager.forceMovement = forwardDir.normalized * rStats.teleportForwardForce;
            }

            PhysicsManager.HandleGravity();
            StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            if(StateManager.CurrentStateFrame == 15)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                CombatManager.Reset();
                return true;
            }
            return false;
        }
    }
}