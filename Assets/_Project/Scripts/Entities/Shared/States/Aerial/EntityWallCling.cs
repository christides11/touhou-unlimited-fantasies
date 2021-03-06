﻿using System.Collections;
using System.Collections.Generic;
using TUF.Entities.Characters;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityWallCling : EntityState
    {
        public override string GetName()
        {
            return "Wall Cling";
        }

        public override void Initialize()
        {
            base.Initialize();
            counter = 0;
            PhysicsManager.forceGravity.y *= controller.EntityStats.wallClingInitFrictionY;
            PhysicsManager.ApplyMovementFriction(controller.EntityStats.wallClingInitFrictionXZ);
            controller.transform.position = PhysicsManager.wallRayHit.point 
                + (PhysicsManager.wallRayHit.normal * 0.51f) + new Vector3(0, -1, 0);
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                PhysicsManager.ApplyMovementFriction(controller.EntityStats.wallClingFrictionXZ);
                EntityStats es = controller.EntityStats;
                PhysicsManager.HandleGravity(es.wallClingMaxFallSpeed, es.wallClingGravity, PhysicsManager.GravityScale);
            }
        }

        int counter;
        public override bool CheckInterrupt()
        {
            if (!PhysicsManager.DetectWall(out int v).transform)
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
