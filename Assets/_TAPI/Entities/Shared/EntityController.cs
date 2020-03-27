using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;
using KinematicCharacterController;
using System;

namespace TAPI.Entities
{
    public class EntityController : SimObject, ILockonable
    {
        public GameManager GameManager { get; protected set; }
        public EntityInput InputManager { get { return entityInput; } }
        public EntityStateManager StateManager { get { return entityStateManager; } }
        public EntityCombatManager CombatManager { get { return entityCombatManager; } }
        public EntityForcesManager ForcesManager { get { return entityForcesManager; } }
        public EntityAnimator EntityAnimator { get { return entityAnimator; } }
        public GameObject LockonTarget { get; protected set; } = null;
        public Vector3 LockonForward { get; protected set; } = Vector3.forward;
        public bool LockonMode { get; protected set; } = false;
        public bool IsGrounded { get; set; } = false;
        public bool IsFloating { get; set; } = false;
        public int Health { get; protected set; } = 0;

        #region References
        [Header("References")]
        [SerializeField] protected EntityInput entityInput;
        [SerializeField] protected EntityStateManager entityStateManager;
        [SerializeField] protected EntityCombatManager entityCombatManager;
        [SerializeField] protected EntityForcesManager entityForcesManager;
        [SerializeField] protected EntityAnimator entityAnimator;
        public EntityDefinition definition;
        public EntityCharacterController cc;
        public CapsuleCollider coll;
        public GameObject visual;
        public Transform lookTransform;
        public Transform visualTransform;
        public TriggerDetector pushbox;
        #endregion

        #region Physics
        [Header("Physics")]
        public float wallCheckDistance = 0.7f;
        [HideInInspector] public GameObject currentWall;
        public float ceilingCheckDistance = 1.2f;
        #endregion

        #region Variables
        [Header("Variables")]
        public LayerMask isGroundedMask;
        public LayerMask attackLayerMask;
        public float groundedCheckRadius = 0.45f;
        public Vector3 groundCheckOffset;
        public Vector3 centerOffset;

        [Header("Lock On")]
        public float lockonRadius;
        public LayerMask lockonLayerMask;
        #endregion

        [HideInInspector] public RaycastHit rayHit;

        public virtual void Init(GameManager gameManager, Transform lookTransform)
        {
            this.GameManager = gameManager;
            this.lookTransform = lookTransform;
        }

        protected override void Awake()
        {
            base.Awake();
            GameManager = TAPI.Core.GameManager.current;
            KinematicCharacterSystem.Settings.AutoSimulation = false;
            pushbox.TriggerStay += ForcesManager.HandlePushForce;
        }

        public override void SimUpdate()
        {
            InputManager.Tick();
            if (CombatManager.hitStop == 0)
            {
                HandleLockon();
                GroundedCheck();
                StateManager.Tick();
                ForcesManager.Tick();
            }
            else
            {
                ForcesManager.SetForceDirect(Vector3.zero, Vector3.zero);
            }
        }

        public override void SimLateUpdate()
        {
            CombatManager.CLateUpdate();
        }

        private void HandleLockon()
        {
            if (InputManager.GetButton(EntityInputs.Lockon).firstPress)
            {
                PickLockonTarget();
                LockonForward = visual.transform.forward;
            }

            bool lockonDown = InputManager.GetButton(EntityInputs.Lockon).isDown;
            LockonMode = lockonDown;
        }

        private void PickLockonTarget()
        {
            LockonTarget = null;
            Physics2D.OverlapCircleAll(transform.position, lockonRadius, lockonLayerMask);
        }

        public virtual Vector3 GetMovementVector(float hozSpeed, float vertSpeed)
        {
            Vector3 forward = lookTransform.forward;
            Vector3 right = lookTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            return forward * vertSpeed + right * hozSpeed;
        }

        public virtual Vector3 GetVisualBasedDirection(Vector3 dir)
        {
            Vector3 vector = visualTransform.TransformDirection(dir);
            return vector;
        }

        public virtual void RotateVisual(Vector3 direction, float speed)
        {
            if (LockonMode)
            {
                return;
            }

            Vector3 newDirection = Vector3.RotateTowards(visual.transform.forward, direction, speed, 0.0f);
            visual.transform.rotation = Quaternion.LookRotation(newDirection);
        }

        public virtual void RotateVisual(Vector3 direction)
        {
            visual.transform.rotation = Quaternion.LookRotation(direction);
        }

        public virtual bool CanAirJump()
        {
            if (InputManager.GetButton(EntityInputs.Jump).firstPress)
            {
                return true;
            }
            return false;
        }

        public virtual bool JumpCancel()
        {
            if (InputManager.GetButton(EntityInputs.Jump).firstPress)
            {
                if (IsGrounded)
                {
                    StateManager.ChangeState((int)EntityStates.JUMP_SQUAT);
                }
                else
                {
                    StateManager.ChangeState((int)EntityStates.AIR_JUMP);
                }
                return true;
            }
            return false;
        }

        public virtual bool DashCancel()
        {
            return false;
        }

        public virtual bool LandCancel()
        {
            if (IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }

        public virtual void FaceDir(Vector3 m, float rotSpeed)
        {
            //Rotate towards our movement force.
            transform.rotation =
                Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m),
                rotSpeed);
        }

        #region Manual Physics Checks
        public virtual void GroundedCheck()
        {
            IsGrounded = cc.Motor.GroundingStatus.IsStableOnGround;
        }

        public virtual bool CeilingAbove()
        {
            bool hit = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), 
                Vector2.up, ceilingCheckDistance, isGroundedMask);
            if (hit)
            {
                return true;
            }
            return false;
        }

        public virtual RaycastHit DetectWall()
        {
            //Get stick direction.
            Vector2 movement = InputManager.GetMovement(0);
            Vector3 translatedMovement = lookTransform.TransformDirection(new Vector3(movement.x, 0, movement.y));
            translatedMovement.y = 0;

            rayHit = new RaycastHit();
            if (translatedMovement.magnitude > InputConstants.movementMagnitude)
            {
                Physics.Raycast(transform.position + new Vector3(0, 1, 0),
                    translatedMovement.normalized, out rayHit, wallCheckDistance, isGroundedMask);
            }
            return rayHit;
        }
        #endregion
    }
}