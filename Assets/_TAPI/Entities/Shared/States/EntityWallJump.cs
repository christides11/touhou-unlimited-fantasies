using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityWallJump : EntityState
    {
        public new static string StateName { get { return "WallJump"; } }

        public override void OnStart()
        {
            controller.IsGrounded = false;
            controller.ForcesManager.forceGravity.y = controller.definition.stats.wallJumpYVelo;

            //Vector2 movement = controller.EntityInput.GetMovement(0).normalized;
            //Vector3 translatedMovement = controller.lookTransform.TransformDirection(new Vector3(movement.x, 0, movement.y));
            //translatedMovement.y = 0;
            Vector3 translatedMovement = controller.rayHit.normal.normalized * controller.definition.stats.wallJumpHVelo;

            controller.ForcesManager.forceMovement = translatedMovement;
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                EntityStats es = controller.definition.stats;
                controller.ForcesManager.ApplyMovement(es.airAcceleration, es.maxAirSpeed, es.airDeceleration, es.airRotationSpeed);

                controller.StateManager.IncrementFrame();
            }
        }
    }
}