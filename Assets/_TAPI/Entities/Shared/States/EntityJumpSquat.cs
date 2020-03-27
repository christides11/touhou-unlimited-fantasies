using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityJumpSquat : EntityState
    {
        public override void OnStart()
        {
            base.OnStart();
            controller.ForcesManager.ApplyMovementFriction();
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                controller.StateManager.IncrementFrame();
            }
        }
    }
}
