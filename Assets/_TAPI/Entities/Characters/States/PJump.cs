﻿using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class PJump : EntityJump
    {

        public override bool CheckInterrupt()
        {
            RaycastHit rh = controller.DetectWall();
            if (controller.CombatManager.CheckForAction())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (controller.CanAirJump())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            if (Mathf.Abs(controller.InputManager.GetFloatDir()) > InputConstants.floatMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.FLOAT);
                return true;
            }
            if (rh.collider)
            {
                controller.currentWall = rh.transform.gameObject;
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_CLING);
                return true;
            }
            if (controller.InputManager.GetButton(EntityInputs.Dash).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if(controller.ForcesManager.forceGravity.y <= 0)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}