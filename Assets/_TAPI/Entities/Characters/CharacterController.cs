using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Characters.States;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters
{
    /// <summary>
    /// The main controller of a character. Unlike the general EntityController,
    /// this controller is assumed to be for entities that the player will control,
    /// and assumes that a few extra states will be used. 
    /// </summary>
    public class CharacterController : EntityController
    {
        [HideInInspector] public bool wasRunning;
        [HideInInspector] public bool hoverMode;

        [Header("Wall Movement")]
        public float sideWallDistance;
        public GameObject lastWall;
        public Vector3 currentWallNormal;
        public int wallSide;

        protected override void Awake()
        {
            base.Awake();
            SetupDefaultStates();
        }

        /// <summary>
        /// Sets up the states and starts the state machine.
        /// </summary>
        protected virtual void SetupDefaultStates()
        {
            // Combat
            StateManager.AddState(new CAttack(), (int)EntityStates.ATTACK);
            StateManager.AddState(new EntityFlinch(), (int)EntityStates.FLINCH);

            // Ground
            StateManager.AddState(new CIdle(), (int)EntityStates.IDLE);
            StateManager.AddState(new CWalk(), (int)EntityStates.WALK);
            StateManager.AddState(new CDash(), (int)EntityStates.DASH);
            StateManager.AddState(new CRun(), (int)EntityStates.RUN);
            StateManager.AddState(new CJumpSquat(), (int)EntityStates.JUMP_SQUAT);

            // Air
            StateManager.AddState(new CJump(), (int)EntityStates.JUMP);
            StateManager.AddState(new CAirJump(), (int)EntityStates.AIR_JUMP);
            StateManager.AddState(new CFall(), (int)EntityStates.FALL);
            StateManager.AddState(new CAirDash(), (int)EntityStates.AIR_DASH);
            StateManager.AddState(new CEnemyStep(), (int)EntityStates.ENEMY_STEP);

            // Float
            StateManager.AddState(new CFloat(), (int)EntityStates.FLOAT);
            StateManager.AddState(new CFloatDodge(), (int)BaseCharacterStates.FLOAT_DODGE);
            StateManager.AddState(new CFloatDash(), (int)BaseCharacterStates.FLOAT_DASH);

            // Walls
            StateManager.AddState(new CWallCling(), (int)BaseCharacterStates.WALL_CLING);
            StateManager.AddState(new CWallJump(), (int)BaseCharacterStates.WALL_JUMP);

            // Start State Machine
            StateManager.ChangeState((int)EntityStates.FALL);
        }

        public virtual bool CheckAirDash()
        {
            if (InputManager.GetButton(EntityInputs.Dash, 0, true).firstPress)
            {
                if (currentAirDash < ((CharacterStats)definition.stats).maxAirDashes)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool DashCancel()
        {
            if (IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.DASH);
                return true;
            }
            else
            {
                if (currentAirDash < ((CharacterStats)definition.stats).maxAirDashes) {
                    StateManager.ChangeState((int)EntityStates.AIR_DASH);
                    return true;
                }
            }
            return false;
        }


        RaycastHit hit;
        public bool CheckForWallsSide()
        {
            Vector2 movement = InputManager.GetMovement(0);
            Vector3 translatedMovement = lookTransform.TransformDirection(new Vector3(movement.x, 0, movement.y));

            Vector3 right = Vector3.Cross(translatedMovement.normalized, Vector3.up.normalized);
            Vector3 left = -right;

            if(Physics.Raycast(transform.position+centerOffset, right, out hit, sideWallDistance, GroundedLayerMask))
            {
                currentWallNormal = hit.normal;
                wallSide = 1;
                return true;
            }

            if(Physics.Raycast(transform.position+centerOffset, left, out hit, sideWallDistance, GroundedLayerMask))
            {
                currentWallNormal = hit.normal;
                wallSide = -1;
                return true;
            }
                
            return false;
        }
    }
}