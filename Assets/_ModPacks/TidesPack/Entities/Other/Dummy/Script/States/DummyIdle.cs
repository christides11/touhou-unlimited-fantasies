using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Entities.Shared;
using TAPI.Entities;

namespace Touhou.Entities
{
    public class DummyIdle : EntityIdle
    {

        public override bool CheckInterrupt()
        {
            if (!controller.IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}