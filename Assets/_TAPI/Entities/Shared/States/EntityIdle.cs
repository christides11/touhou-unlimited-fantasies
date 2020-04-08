using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityIdle : EntityState
    {
        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }
            controller.ForcesManager.ApplyMovementFriction();
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