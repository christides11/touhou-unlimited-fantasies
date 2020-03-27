using System.Collections;
using System.Collections.Generic;
using TAPI.Entities.Characters.States;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Entities.Characters
{
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

        protected virtual void SetupDefaultStates()
        {
            PIdle idle = new PIdle();
            StateManager.AddState(idle, (int)EntityStates.IDLE);
            PWalk walk = new PWalk();
            StateManager.AddState(walk, (int)EntityStates.WALK);
            PDash dash = new PDash();
            StateManager.AddState(dash, (int)EntityStates.DASH);
            PRun run = new PRun();
            StateManager.AddState(run, (int)EntityStates.RUN);
            PFall fall = new PFall();
            StateManager.AddState(fall, (int)EntityStates.FALL);
            PJump jump = new PJump();
            StateManager.AddState(jump, (int)EntityStates.JUMP);
            PJumpSquat jumpSquat = new PJumpSquat();
            StateManager.AddState(jumpSquat, (int)EntityStates.JUMP_SQUAT);
            PAirDash airDash = new PAirDash();
            StateManager.AddState(airDash, (int)EntityStates.AIR_DASH);
            PWallCling wallCling = new PWallCling();
            StateManager.AddState(wallCling, (int)BaseCharacterStates.WALL_CLING);
            PWallJump wallJump = new PWallJump();
            StateManager.AddState(wallJump, (int)BaseCharacterStates.WALL_JUMP);
            PFloat aFloat = new PFloat();
            StateManager.AddState(aFloat, (int)EntityStates.FLOAT);
            PAirJump airJump = new PAirJump();
            StateManager.AddState(airJump, (int)EntityStates.AIR_JUMP);
            PFloatDodge floatDodge = new PFloatDodge();
            StateManager.AddState(floatDodge, (int)BaseCharacterStates.FLOAT_DODGE);
            PFloatDash floatDash = new PFloatDash();
            StateManager.AddState(floatDash, (int)BaseCharacterStates.FLOAT_DASH);
            PAttack attack = new PAttack();
            StateManager.AddState(attack, (int)EntityStates.ATTACK);
            EntityFlinch flinch = new EntityFlinch();
            StateManager.AddState(flinch, (int)EntityStates.FLINCH);

            StateManager.ChangeState((int)EntityStates.FALL);
        }

        #region Walls
        RaycastHit hit;
        public bool CheckForWallsSide()
        {
            Vector2 movement = InputManager.GetMovement(0);
            Vector3 translatedMovement = lookTransform.TransformDirection(new Vector3(movement.x, 0, movement.y));

            Vector3 right = Vector3.Cross(translatedMovement.normalized, Vector3.up.normalized);
            Vector3 left = -right;

            if(Physics.Raycast(transform.position+centerOffset, right, out hit, sideWallDistance, isGroundedMask))
            {
                currentWallNormal = hit.normal;
                wallSide = 1;
                return true;
            }

            if(Physics.Raycast(transform.position+centerOffset, left, out hit, sideWallDistance, isGroundedMask))
            {
                currentWallNormal = hit.normal;
                wallSide = -1;
                return true;
            }
                
            return false;
        }
        #endregion
    }
}