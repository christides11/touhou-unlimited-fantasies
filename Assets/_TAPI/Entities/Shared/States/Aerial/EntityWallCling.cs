using System.Collections;
using System.Collections.Generic;
using TAPI.Entities.Characters;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityWallCling : EntityState
    {
        public override void OnStart()
        {
            base.OnStart();
            counter = 0;
            PhysicsManager.forceGravity.y *= controller.definition.stats.wallClingInitFrictionY;
            PhysicsManager.ApplyMovementFriction(controller.definition.stats.wallClingInitFrictionXZ);
            controller.transform.position = PhysicsManager.wallRayHit.point 
                + (PhysicsManager.wallRayHit.normal * 0.51f) + new Vector3(0, -1, 0);
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                PhysicsManager.ApplyMovementFriction(controller.definition.stats.wallClingFrictionXZ);
                EntityStats es = controller.definition.stats;
                PhysicsManager.HandleGravity(es.wallClingMaxFallSpeed, es.wallClingGravity, PhysicsManager.CurrentGravityScale);
            }
        }

        int counter;
        public override bool CheckInterrupt()
        {
            if (!PhysicsManager.DetectWall().transform)
            {
                counter++;
                if (counter > 4)
                {
                    PhysicsManager.currentWall = null;
                    StateManager.ChangeState((int)EntityStates.FALL);
                    return true;
                }
            }
            else
            {
                counter = 0;
            }
            return false;
        }

        public override void OnInterrupted()
        {
            counter = 0;
        }
    }
}
