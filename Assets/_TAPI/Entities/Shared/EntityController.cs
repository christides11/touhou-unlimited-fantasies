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
        public bool LockedOn { get; protected set; } = false;
        public Vector3 LockonForward { get; protected set; } = Vector3.forward;
        public bool IsGrounded { get; set; } = false;
        public bool IsFloating { get; set; } = false;
        public int Health { get; protected set; } = 0;
        public bool Lockonable { get; protected set; } = true;

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
        public float lockonFudging = 0.1f;
        public LayerMask lockonLayerMask;
        public LayerMask visibilityLayerMask;
        #endregion

        [HideInInspector] public RaycastHit rayHit;

        /// <summary>
        /// Initializes the entity with the references needed.
        /// </summary>
        /// <param name="gameManager">The global game manager.</param>
        /// <param name="lookTransform">The "Camera" transform that this entity uses.</param>
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

        /// <summary>
        /// Called every simulation tick. 
        /// </summary>
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

        /// <summary>
        /// Handles finding and locking on to targets.
        /// </summary>
        private void HandleLockon()
        {
            InputRecordButton lockonButton = InputManager.GetButton(EntityInputs.Lockon);

            LockedOn = false;
            if (!lockonButton.isDown)
            {
                LockonTarget = null;
                return;
            }
            LockedOn = true;

            if (lockonButton.firstPress)
            {
                PickLockonTarget();
                // No target but holding down lock on menas you lock the visuals rotation.
                LockonForward = visual.transform.forward;
            }

            // No target.
            if(LockonTarget == null)
            {
                return;
            }

            // Target out of range.
            if(Vector3.Distance(transform.position, LockonTarget.transform.position) > lockonRadius)
            {
                LockonTarget = null;
                return;
            }

            // We have a target and they're in range, set our wanted forward direction.
            Vector3 dir = (LockonTarget.transform.position - transform.position);
            dir.y = 0;
            LockonForward = dir.normalized;
        }

        /// <summary>
        /// Picks the best lockon target based on stick direction.
        /// </summary>
        private void PickLockonTarget()
        {
            LockonTarget = null;
            Collider[] list = Physics.OverlapSphere(transform.position, lockonRadius, lockonLayerMask);

            // The direction of the lockon defaults to the forward of the camera.
            Vector3 referenceDirection = GetMovementVector(0, 1);
            // If the movement stick is pointing in a direction, then our lockon should
            // be based on that angle instead.
            Vector2 movementDir = InputManager.GetMovement(0);
            if (movementDir.magnitude >= InputConstants.movementMagnitude)
            {
                referenceDirection = GetMovementVector(movementDir.x, movementDir.y);
            }

            // Loop through all targets and find the one that matches the angle the best.
            GameObject closestTarget = null;
            float closestAngle = -1.1f;
            float closestDistance = Mathf.Infinity;
            foreach(Collider c in list)
            {
                // Ignore self.
                if (c.gameObject == gameObject)
                {
                    continue;
                }
                // Only objects with ILockonable can be locked on to.
                if (c.TryGetComponent(out ILockonable lockonComponent))
                {
                    // The target can not be locked on to right now.
                    if (!lockonComponent.Lockonable)
                    {
                        continue;
                    }
                    Vector3 targetDistance = (c.transform.position - transform.position);
                    // If we can't see the target, it can not be locked on to.
                    if(!Physics.Raycast(transform.position, targetDistance.normalized, lockonRadius, visibilityLayerMask))
                    {
                        continue;
                    }
                    targetDistance.y = 0;
                    float currAngle = Vector3.Dot(referenceDirection, targetDistance.normalized);
                    bool withinFudging = Mathf.Abs(currAngle - closestAngle) <= lockonFudging;
                    // Targets have similar positions, choose the closer one.
                    if(withinFudging)
                    {
                        if(targetDistance.sqrMagnitude < closestDistance)
                        {
                            closestTarget = c.gameObject;
                            closestAngle = currAngle;
                            closestDistance = targetDistance.sqrMagnitude;
                        }
                    }
                    // Target is closer to the angle than the last one, this is the new target.
                    else if (currAngle > closestAngle)
                    {
                        closestTarget = c.gameObject;
                        closestAngle = currAngle;
                        closestDistance = targetDistance.sqrMagnitude;
                    }
                }
            }

            if(closestTarget != null)
            {
                LockonTarget = closestTarget;
            }
        }

        /// <summary>
        /// Translates the movement vector based on the look transform's
        /// forward.
        /// </summary>
        /// <param name="frame">The frame we want to check the movement input for.</param>
        /// <returns>A direction vector based on the camera's forward.</returns>
        public virtual Vector3 GetMovementVector(int frame = 0)
        {
            Vector2 movement = InputManager.GetMovement(frame);
            return GetMovementVector(movement.x, movement.y);
        }

        /// <summary>
        /// Takes a vector of the wanted movement and translate it to the look
        /// transform's forward.
        /// </summary>
        /// <param name="hozSpeed"></param>
        /// <param name="vertSpeed"></param>
        /// <returns>A direction vector based on the camera's forward.</returns>
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

        /// <summary>
        /// Transforms the given direction into one based on the visual's forward.
        /// </summary>
        /// <param name="dir">The direction vector.</param>
        /// <returns>A direction vector based on the visual's forward.</returns>
        public virtual Vector3 GetVisualBasedDirection(Vector3 dir)
        {
            Vector3 vector = visualTransform.TransformDirection(dir);
            return vector;
        }

        /// <summary>
        /// Rotate the visual towards the given direction based on the speed given.
        /// </summary>
        /// <param name="direction">The direction to rotate towards.</param>
        /// <param name="speed">The speed of the rotation.</param>
        public virtual void RotateVisual(Vector3 direction, float speed)
        {
            Vector3 newDirection = Vector3.RotateTowards(visual.transform.forward, direction, speed, 0.0f);
            visual.transform.rotation = Quaternion.LookRotation(newDirection);
        }

        /// <summary>
        /// Set the visual's rotation to the given direction.
        /// </summary>
        /// <param name="direction">The direction to set the rotation to.</param>
        public virtual void SetVisualRotation(Vector3 direction)
        {
            visual.transform.rotation = Quaternion.LookRotation(direction);
        }

        /// <summary>
        /// If the entity can currently air jump.
        /// </summary>
        /// <returns>True if the entity can air jump currently.</returns>
        public virtual bool CanAirJump()
        {
            if (InputManager.GetButton(EntityInputs.Jump).firstPress)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        // Tries to jump cancel if possible.
        /// </summary>
        /// <returns>True if the jump cancel was successful.</returns>
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

        /// <summary>
        /// Tries to dash if possible.
        /// </summary>
        /// <returns>True if the dash cancel was successful.</returns>
        public virtual bool DashCancel()
        {
            return false;
        }

        /// <summary>
        /// Tries to land cancel if possible.
        /// </summary>
        /// <returns>True if the land cancel was successful.</returns>
        public virtual bool LandCancel()
        {
            if (IsGrounded)
            {
                StateManager.ChangeState((int)EntityStates.IDLE);
                return true;
            }
            return false;
        }

        #region Manual Physics Checks
        /// <summary>
        /// Check if we are on the ground.
        /// </summary>
        public virtual void GroundedCheck()
        {
            IsGrounded = cc.Motor.GroundingStatus.IsStableOnGround;
        }

        /// <summary>
        /// Check if there is something above us.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Check if there's a wall in the movement direction we're pointing.
        /// </summary>
        /// <returns>The RaycastHit result.</returns>
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