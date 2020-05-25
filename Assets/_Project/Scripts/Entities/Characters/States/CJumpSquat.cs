using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CJumpSquat : EntityJumpSquat
    {
        public override bool CheckInterrupt()
        {
            if (controller.StateManager.CurrentStateFrame >= controller.definition.stats.jumpsquat)
            {
                controller.StateManager.ChangeState((int)EntityStates.JUMP);
                return true;
            }
            return false;
        }
    }
}
