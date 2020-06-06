using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Characters.States;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.Entities.Characters
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
        public int currentAirDash = 0;

        [Header("Wall Movement")]
        public float sideWallDistance;
        public GameObject lastWall;
        public RaycastHit lastWallHit;
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
            //StateManager.AddState(new CWallCling(), (int)BaseCharacterStates.WALL_CLING);
            StateManager.AddState(new CWallJump(), (int)BaseCharacterStates.WALL_JUMP);
            StateManager.AddState(new CVertWallRun(), (int)BaseCharacterStates.WALL_RUN_VERTICAL);
            StateManager.AddState(new CHozWallRun(), (int)BaseCharacterStates.WALL_RUN_HORIZONTAL);

            // Start State Machine
            StateManager.ChangeState((int)EntityStates.FALL);
        }

        public virtual bool CheckAirDash()
        {
            if (InputManager.GetButton((int)EntityInputs.Dash, 0, true).firstPress)
            {
                if (currentAirDash < ((CharacterStats)definition.stats).maxAirDashes)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool TryDash()
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

        public virtual bool TryWallRun()
        {
            if (InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude >= InputConstants.movementMagnitude)
            {
                RaycastHit rh = ((EntityPhysicsManager)PhysicsManager).DetectWall();
                if (rh.collider)
                {
                    if (Vector3.Dot(rh.normal, GetMovementVector()) < -0.9f)
                    {
                        lastWallHit = rh;
                        StateManager.ChangeState((int)BaseCharacterStates.WALL_RUN_VERTICAL);
                        return true;
                    }
                    else if (Vector3.Dot(rh.normal, GetMovementVector()) < -0.2f)
                    {
                        lastWallHit = rh;
                        StateManager.ChangeState((int)BaseCharacterStates.WALL_RUN_HORIZONTAL);
                        return true;
                    }
                }
            }
            return false;
        }

        public override void ResetAirActions()
        {
            base.ResetAirActions();
            currentAirDash = 0;
        }
    }
}