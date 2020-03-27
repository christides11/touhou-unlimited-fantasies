using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityFall : EntityState
    {
        public new static string StateName { get { return "Fall"; } }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                EntityStats es = controller.definition.stats;
                controller.ForcesManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration, es.airRotationSpeed);
            }
        }
    }
}
