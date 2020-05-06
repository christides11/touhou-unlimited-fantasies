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
        public EntityPhysicsManager PhysicsManager { get { return entityPhysicsManager; } }
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
        [SerializeField] protected EntityPhysicsManager entityPhysicsManager;
        [SerializeField] protected EntityAnimator entityAnimator;
        public EntityDefinition definition;
        public EntityCharacterController cc;
        public CapsuleCollider coll;
        public GameObject visual;
        public Transform lookTransform;
        public Transform visualTransform;
        public TriggerDetector pushbox;
        #endregion

        [Header("Variables")]
        public LayerMask GroundedLayerMask;
        public LayerMask attackLayerMask;
        public Vector3 groundCheckOffset;
        public Vector3 centerOffset;

        [Header("Lock On")]
        public float softLockonRadius;
        public float lockonRadius;
        public float lockonFudging = 0.1f;
        public LayerMask lockonLayerMask;
        public LayerMask visibilityLayerMask;

        [Header("Enemy Step")]
        public LayerMask enemyStepLayerMask;
        public float enemyStepCheckRadius;

        [Header("Stuff")]
        public int currentAirJump = 0;
        public int currentAirDash = 0;

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
            pushbox.TriggerStay += PhysicsManager.HandlePushForce;
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
                PhysicsManager.GroundedCheck();
                StateManager.Tick();
                PhysicsManager.Tick();
            }
            else
            {
                PhysicsManager.SetForceDirect(Vector3.zero, Vector3.zero);
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
        /// Picks the best soft lockon target based on distance.
        /// </summary>
        public void PickSoftlockTarget()
        {
            if (LockedOn || InputManager.GetMovement(0).magnitude < InputConstants.movementMagnitude)
            {
                return;
            }
            LockonTarget = null;
            Collider[] list = Physics.OverlapSphere(transform.position, softLockonRadius, lockonLayerMask);

            float closestDistance = softLockonRadius;
            foreach (Collider c in list)
            {
                // Ignore self.
                if (c.gameObject == gameObject)
                {
                    continue;
                }
                // Only objects with ILockonable can be locked on to.
                if (c.TryGetComponent(out ILockonable lockonComponent))
                {
                    if(Vector3.Distance(transform.position, c.transform.position) < closestDistance)
                    {
                        LockonTarget = c.gameObject;
                    }
                }
            }
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
                    if(Physics.Raycast(transform.position, targetDistance.normalized, lockonRadius, visibilityLayerMask))
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
        /// Translates the movement vector based on the look transform's forward.
        /// </summary>
        /// <param name="frame">The frame we want to check the movement input for.</param>
        /// <returns>A direction vector based on the camera's forward.</returns>
        public virtual Vector3 GetMovementVector(int frame = 0)
        {
            Vector2 movement = InputManager.GetMovement(frame);
            return GetMovementVector(movement.x, movement.y);
        }

        /// <summary>
        /// Translates the given vector based on the look transform's forward.
        /// </summary>
        /// <param name="horizontal">The horizontal axis of the vector.</param>
        /// <param name="vertical">The vertical axis of the vector.</param>
        /// <returns>A direction vector based on the camera's forward.</returns>
        public virtual Vector3 GetMovementVector(float horizontal, float vertical)
        {
            if(lookTransform == null)
            {
                return Vector3.forward;
            }
            Vector3 forward = lookTransform.forward;
            Vector3 right = lookTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            return forward * vertical + right * horizontal;
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
        public virtual bool CheckAirJump()
        {
            if (InputManager.GetButton(EntityInputs.Jump).firstPress)
            {
                if (currentAirJump < definition.stats.maxAirJumps)
                {
                    return true;
                }
            }
            return false;
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

        public virtual bool EnemyStepCancel()
        {
            if (InputManager.GetButton(EntityInputs.Jump, 0, true).firstPress)
            {
                Collider[] c = Physics.OverlapSphere(transform.position, enemyStepCheckRadius, enemyStepLayerMask);
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i].gameObject != pushbox.gameObject)
                    {
                        StateManager.ChangeState((int)EntityStates.ENEMY_STEP);
                        return true;
                    }
                }
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

        public virtual void ResetAirActions()
        {
            PhysicsManager.CurrentGravityScale = 1.0f;
            currentAirJump = 0;
            currentAirDash = 0;
        }
    }
}