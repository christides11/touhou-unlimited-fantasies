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
            controller.PhysicsManager.ApplyGravity = false;
            controller.PhysicsManager.forceGravity.y *= controller.definition.stats.wallClingInitFrictionY;
            controller.PhysicsManager.ApplyMovementFriction(controller.definition.stats.wallClingInitFrictionXZ);
            controller.transform.position = controller.PhysicsManager.wallRayHit.point 
                + (controller.PhysicsManager.wallRayHit.normal * 0.51f) + new Vector3(0, -1, 0);
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                controller.PhysicsManager.ApplyMovementFriction(controller.definition.stats.wallClingFrictionXZ);
                EntityStats es = controller.definition.stats;
                controller.PhysicsManager.forceGravity.y -= es.wallClingGravity;
                if(controller.PhysicsManager.forceGravity.y < -es.wallClingMaxFallSpeed)
                {
                    controller.PhysicsManager.forceGravity.y = -es.wallClingMaxFallSpeed;
                }
            }
        }

        int counter;
        public override bool CheckInterrupt()
        {
            if (!controller.PhysicsManager.DetectWall().transform)
            {
                counter++;
                if (counter > 4)
                {
                    controller.PhysicsManager.currentWall = null;
                    controller.StateManager.ChangeState((int)EntityStates.FALL);
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
            controller.PhysicsManager.ApplyGravity = true;
        }
    }
}
