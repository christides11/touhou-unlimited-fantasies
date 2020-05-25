using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Entities.Shared;
using TUF.Entities.Characters;
using TUF.Entities;

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
