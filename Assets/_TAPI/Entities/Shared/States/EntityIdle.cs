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
        }

        public override string GetName()
        {
            return "Idle";
        }
    }
}