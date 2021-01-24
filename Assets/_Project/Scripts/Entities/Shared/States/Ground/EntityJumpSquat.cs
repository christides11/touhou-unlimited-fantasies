using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Shared
{
    public class EntityJumpSquat : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
            controller.fullHop = true;
            //PhysicsManager.ApplyMovementFriction();
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
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).released)
            {
                controller.fullHop = false;
            }
            CheckInterrupt();
            controller.StateManager.IncrementFrame();
        }
    }
}