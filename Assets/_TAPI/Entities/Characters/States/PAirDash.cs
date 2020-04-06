﻿using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters.States
{
    public class PAirDash : CharacterState
    {
        public override void OnStart()
        {
            base.OnStart();
            controller.ForcesManager.ApplyGravity = false;
            controller.ForcesManager.forceGravity = Vector3.zero;
            Vector2 movement = controller.InputManager.GetMovement(0);
            if(movement.magnitude == 0)
            {
                movement = Vector2.up;
            }
            Vector3 translatedMovement = controller.GetMovementVector(movement.x, movement.y);
            translatedMovement *= ((CharacterStats)controller.definition.stats).airDashVelo;

            controller.ForcesManager.forceMovement = translatedMovement;
            controller.RotateVisual(translatedMovement, 100);
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                CharacterStats ps = ((CharacterStats)controller.definition.stats);
                if (controller.StateManager.CurrentStateFrame > ps.airDashHoldVelo)
                {
                    controller.ForcesManager.ApplyMovementFriction(ps.airDashFriction);
                }
                controller.StateManager.IncrementFrame();
            }
        }

        public override bool CheckInterrupt()
        {
            RaycastHit rh = controller.DetectWall();
            if (rh.collider)
            {
                controller.currentWall = rh.transform.gameObject;
                controller.StateManager.ChangeState((int)BaseCharacterStates.WALL_CLING);
                return true;
            }
            if (Mathf.Abs(controller.InputManager.GetFloatDir()) > InputConstants.floatMagnitude)
            {
                controller.StateManager.ChangeState((int)EntityStates.FLOAT);
                return true;
            }
            if (controller.CanAirJump())
            {
                controller.StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                return true;
            }
            if (controller.StateManager.CurrentStateFrame >= ((CharacterStats)controller.definition.stats).airDashLength)
            {
                controller.StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            if (controller.IsGrounded)
            {
                controller.StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            controller.ForcesManager.ApplyGravity = true;
        }

        public override string GetName()
        {
            return "Air Dash";
        }
    }
}