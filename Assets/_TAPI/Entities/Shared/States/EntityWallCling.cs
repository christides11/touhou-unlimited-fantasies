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
            controller.ForcesManager.ApplyGravity = false;
            controller.ForcesManager.forceGravity.y *= controller.definition.stats.wallClingInitFrictionY;
            controller.ForcesManager.ApplyMovementFriction(controller.definition.stats.wallClingInitFrictionXZ);
            controller.transform.position = controller.rayHit.point + (controller.rayHit.normal * 0.51f) + new Vector3(0, -1, 0);
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                controller.ForcesManager.ApplyMovementFriction(controller.definition.stats.wallClingFrictionXZ);
                EntityStats es = controller.definition.stats;
                controller.ForcesManager.forceGravity.y -= es.wallClingGravity;
                if(controller.ForcesManager.forceGravity.y < -es.wallClingMaxFallSpeed)
                {
                    controller.ForcesManager.forceGravity.y = -es.wallClingMaxFallSpeed;
                }
            }
        }

        int counter;
        public override bool CheckInterrupt()
        {
            if (!controller.DetectWall().transform)
            {
                counter++;
                if (counter > 4)
                {
                    controller.currentWall = null;
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
            controller.ForcesManager.ApplyGravity = true;
        }
    }
}
