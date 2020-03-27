using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Entities.Shared;
using TAPI.Entities.Characters;
using TAPI.Entities;

namespace Touhou.Entities
{
    public class DummyFall : EntityFall
    {
        public override bool CheckInterrupt()
        {
            if (controller.IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }
    }
}
