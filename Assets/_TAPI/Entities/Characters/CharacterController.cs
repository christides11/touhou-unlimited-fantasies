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
            StateManager.AddState(new PAttack(), (int)EntityStates.ATTACK);
            StateManager.AddState(new EntityFlinch(), (int)EntityStates.FLINCH);

            // Ground
            StateManager.AddState(new PIdle(), (int)EntityStates.IDLE);
            StateManager.AddState(new PWalk(), (int)EntityStates.WALK);
            StateManager.AddState(new PDash(), (int)EntityStates.DASH);
            StateManager.AddState(new PRun(), (int)EntityStates.RUN);
            StateManager.AddState(new PJumpSquat(), (int)EntityStates.JUMP_SQUAT);

            // Air
            StateManager.AddState(new PJump(), (int)EntityStates.JUMP);
            StateManager.AddState(new PAirJump(), (int)EntityStates.AIR_JUMP);
            StateManager.AddState(new PFall(), (int)EntityStates.FALL);
            StateManager.AddState(new PAirDash(), (int)EntityStates.AIR_DASH);
            StateManager.AddState(new PEnemyStep(), (int)EntityStates.ENEMY_STEP);

            // Float
            StateManager.AddState(new PFloat(), (int)EntityStates.FLOAT);
            StateManager.AddState(new PFloatDodge(), (int)BaseCharacterStates.FLOAT_DODGE);
            StateManager.AddState(new PFloatDash(), (int)BaseCharacterStates.FLOAT_DASH);

            // Walls
            StateManager.AddState(new PWallCling(), (int)BaseCharacterStates.WALL_CLING);
            StateManager.AddState(new PWallJump(), (int)BaseCharacterStates.WALL_JUMP);

            // Start State Machine
            StateManager.ChangeState((int)EntityStates.FALL);
        }

        /// <summary>
        /// If the entity can currently air jump.
        /// </summary>
        /// <returns>True if the entity can air jump currently.</returns>
        public virtual bool CheckAirDash()
        {
            if (InputManager.GetButton(EntityInputs.Dash).firstPress)
            {
                //if (currentAirJump < definition.stats.airdas)
                //{
                return true;
                //}
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