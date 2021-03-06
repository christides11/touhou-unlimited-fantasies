﻿using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CWallCling : EntityWallCling
    {
        public override bool CheckInterrupt()
        {
            if (controller.InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_JUMP);
                return true;
            }
            return base.CheckInterrupt();
        }
    }
}