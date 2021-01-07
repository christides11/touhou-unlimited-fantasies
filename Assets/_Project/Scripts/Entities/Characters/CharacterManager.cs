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
    public class CharacterManager : EntityManager
    {
        [HideInInspector] public bool wasRunning;
        [HideInInspector] public bool hoverMode;
        public int currentAirDash = 0;

        [Header("Wall Movement")]
        public float sideWallDistance;
        public int wallSide;
        public float wallRunHozMultiplier = 1.0f;

        /// <summary>
        /// Sets up the states and starts the state machine.
        /// </summary>
        protected override void SetupDefaultStates()
        {
            // Combat
            StateManager.AddState(new CAttack(), (int)EntityStates.ATTACK);

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
            StateManager.AddState(new CVertWallRun(), (int)BaseCharacterStates.WALL_RUN_VERTICAL);
            StateManager.AddState(new CHozWallRun(), (int)BaseCharacterStates.WALL_RUN_HORIZONTAL);

            // Hit Reactions
            StateManager.AddState(new EntityFlinch(), (int)EntityStates.FLINCH);
            StateManager.AddState(new EntityFlinchAir(), (int)EntityStates.FLINCH_AIR);
            StateManager.AddState(new EntityTumble(), (int)EntityStates.TUMBLE);
            StateManager.AddState(new EntityWallBounce(), (int)EntityStates.WALL_BOUNCE);

            // Other
            StateManager.AddState(new CSlide(), (int)BaseCharacterStates.SLIDE);
            StateManager.AddState(new CLedgeHang(), (int)BaseCharacterStates.LEDGE_HANG);
            StateManager.AddState(new CLedgeJump(), (int)BaseCharacterStates.LEDGE_JUMP);

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

        public float wallRunVertical = -0.9f;
        public virtual bool TryWallRun()
        {
            if (InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude >= InputConstants.movementMagnitude)
            {
                RaycastHit rh = ((EntityPhysicsManager)PhysicsManager).DetectWall(out wallSide);
                if (rh.collider)
                {
                    float dotProduct = Vector3.Dot(rh.normal, GetMovementVector().normalized);
                    if (dotProduct < wallRunVertical)
                    {
                        lastWallHit = rh;
                        StateManager.ChangeState((int)BaseCharacterStates.WALL_RUN_VERTICAL);
                        return true;
                    }
                    else if (dotProduct < -0.1f)
                    {
                        lastWallHit = rh;
                        StateManager.ChangeState((int)BaseCharacterStates.WALL_RUN_HORIZONTAL);
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool TryWallCling()
        {
            if (InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude >= InputConstants.movementMagnitude)
            {
                RaycastHit rh = ((EntityPhysicsManager)PhysicsManager).DetectWall(out wallSide);
                if (rh.collider)
                {
                    StateManager.ChangeState((int)BaseCharacterStates.WALL_CLING);
                }
            }
            return false;
        }

        public Vector3 ledgeRayOffset;
        public float ledgeRayDistance;
        public float ledgeSphereRadius;
        public float ledgeDistance;

        public Vector3 lastLedgePosition;
        public virtual bool TryLedgeGrab()
        {
            if(GetMovementVector().magnitude < InputConstants.movementMagnitude)
            {
                return false;
            }

            RaycastHit hitInfo;
            Physics.SphereCast(transform.position + ledgeRayOffset, ledgeSphereRadius, visualTransform.forward, out hitInfo, ledgeRayDistance);
            if (hitInfo.collider != null)
            {
                if(hitInfo.collider.TryGetComponent<Ledge>(out Ledge l))
                {
                    Vector3 closestLedge = l.FindClosestLedge(transform.position + ledgeRayOffset);
                    if(Vector3.Distance(transform.position+ledgeRayOffset, closestLedge) <= ledgeDistance)
                    {
                        lastLedgePosition = closestLedge;
                        StateManager.ChangeState((int)BaseCharacterStates.LEDGE_HANG);
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
            wallRunHozMultiplier = 1.0f;
        }
    }
}