using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class PWallCling : EntityWallCling
    {
        public override bool CheckInterrupt()
        {
            if (controller.InputManager.GetButton(EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_JUMP);
                return true;
            }
            return base.CheckInterrupt();
        }
    }
}