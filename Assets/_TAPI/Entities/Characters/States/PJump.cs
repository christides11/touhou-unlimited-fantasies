using System.Collections;
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
            RaycastHit rh = controller.PhysicsManager.DetectWall();
            if (controller.CombatManager.CheckForAction())
            {
                controller.StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (controller.EnemyStepCancel())
            {
                return true;
            }
            if (controller.CheckAirJump())
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
                controller.PhysicsManager.currentWall = rh.transform.gameObject;
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_CLING);
                return true;
            }
            if (controller.InputManager.GetButton(EntityInputs.Dash, 0, true).firstPress)
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if(controller.PhysicsManager.forceGravity.y <= 0)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}
