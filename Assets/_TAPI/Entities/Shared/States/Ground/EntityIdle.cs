using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityIdle : EntityState
    {
        public override void OnStart()
        {
            base.OnStart();
            controller.ResetAirActions();
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }
            controller.PhysicsManager.ApplyMovementFriction();
            if(controller.LockedOn)
            {
                controller.RotateVisual(controller.LockonForward, 10);
            }
        }

        public override string GetName()
        {
            return "Idle";
        }
    }
}