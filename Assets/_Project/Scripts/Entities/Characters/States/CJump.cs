using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CJump : EntityJump
    {

        public override bool CheckInterrupt()
        {
            if (CombatManager.TryAttack())
            {
                StateManager.ChangeState((int)EntityStates.ATTACK);
                return true;
            }
            if (controller.TryEnemyStep())
            {
                return true;
            }
            if (controller.CanAirJump())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            if (Mathf.Abs(controller.InputManager.GetAxis((int)EntityInputs.Float)) > InputConstants.floatMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.FLOAT);
                return true;
            }
            if (Controller.InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude >= InputConstants.movementMagnitude)
            {
                RaycastHit rh = PhysicsManager.DetectWall();
                if (rh.collider)
                {
                    if (Vector3.Dot(rh.normal, controller.GetMovementVector()) < -0.85f)
                    {
                        ((CharacterController)controller).lastWallHit = rh;
                        StateManager.ChangeState((int)BaseCharacterStates.WALL_RUN_VERTICAL);
                        return true;
                    }
                    else if (Vector3.Dot(rh.normal, controller.GetMovementVector()) < -0.2f)
                    {
                        ((CharacterController)controller).lastWallHit = rh;
                        StateManager.ChangeState((int)BaseCharacterStates.WALL_RUN_HORIZONTAL);
                        return true;
                    }
                }
            }
            if (((CharacterController)controller).CheckAirDash())
            {
                StateManager.ChangeState((int)EntityStates.AIR_DASH);
                return true;
            }
            if(PhysicsManager.forceGravity.y <= 0)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}
