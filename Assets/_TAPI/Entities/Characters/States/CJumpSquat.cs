using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
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
