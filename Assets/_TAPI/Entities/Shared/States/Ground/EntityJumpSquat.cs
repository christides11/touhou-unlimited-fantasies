using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    public class EntityJumpSquat : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            controller.fullHop = true;
            controller.PhysicsManager.ApplyMovementFriction();
            if (controller.LockedOn)
            {
                controller.SetVisualRotation(controller.LockonForward);
            }
            else
            {
                Vector3 lookVector = controller.GetMovementVector();
                if (lookVector.magnitude >= InputConstants.movementMagnitude)
                {
                    controller.SetVisualRotation(lookVector);
                }
            }
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                if (controller.InputManager.GetButton((int)EntityInputs.Jump).released)
                {
                    controller.fullHop = false;
                }

                controller.StateManager.IncrementFrame();
            }
        }
    }
}