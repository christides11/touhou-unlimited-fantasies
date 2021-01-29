using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Entities.Characters.States {
    public class CLedgeHang : EntityState
    {
        Vector3 savedForce;
        public override void Initialize()
        {
            base.Initialize();
            savedForce = PhysicsManager.forceMovement;
            PhysicsManager.forceMovement = Vector3.zero;
            PhysicsManager.forceGravity = Vector3.zero;
        }

        public override void OnUpdate()
        {
            controller.StateManager.IncrementFrame();
            CheckInterrupt();
        }

        public override bool CheckInterrupt()
        {
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.cc.Motor.SetPosition(((CharacterManager)controller).lastLedgePosition + new Vector3(0, 0.2f, 0));
                if(controller.StateManager.CurrentStateFrame <= 5)
                {
                    PhysicsManager.forceMovement = savedForce;
                }
                controller.StateManager.ChangeState((int)BaseCharacterStates.LEDGE_JUMP);
                return true;
            }
            return false;
        }

        public override string GetName()
        {
            return "Ledge Hang";
        }
    }
}
