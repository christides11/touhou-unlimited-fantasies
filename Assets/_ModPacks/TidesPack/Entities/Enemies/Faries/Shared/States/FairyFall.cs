using System.Collections;
using System.Collections.Generic;
using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;

namespace TidesPack.Enemies.Fairies
{
    public class FairyFall : EntityFall
    {
        public override string GetName()
        {
            return "Fall";
        }

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