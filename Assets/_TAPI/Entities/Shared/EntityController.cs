﻿using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;
using KinematicCharacterController;
using System;
using CAF.Input;

namespace TAPI.Entities
{
    /// <summary>
    /// A general controller that should work for a majority of entities.
    /// </summary>
    public class EntityController : CAF.Entities.EntityController
    {
        public new EntityInputManager InputManager { get { return entityInput; } }
        public new EntityPhysicsManager PhysicsManager { get { return entityPhysicsManager; } }

        public GameManager GameManager { get; protected set; }
        public EntityAnimator EntityAnimator { get { return entityAnimator; } }
        public GameObject LockonTarget { get; protected set; } = null;
        public bool LockedOn { get; protected set; } = false;
        public Vector3 LockonForward { get; protected set; } = Vector3.forward;
        public bool IsFloating { get; set; } = false;
        public int Health { get; protected set; } = 0;
        public bool Lockonable { get; protected set; } = true;

        public new Vector3 Center { get { return pushbox.Collider.bounds.center; } }

        #region References
        [Header("References")]
        [SerializeField] protected new EntityInputManager entityInput;
        [SerializeField] protected new EntityPhysicsManager entityPhysicsManager;
        [SerializeField] protected EntityAnimator entityAnimator;
        public EntityDefinition definition;
        public EntityCharacterController cc;
        public Transform lookTransform;
        public Transform visualTransform;
        public TriggerDetector pushbox;
        #endregion

        [Header("Variables")]
        public LayerMask GroundedLayerMask;
        public LayerMask attackLayerMask;
        public Vector3 groundCheckOffset;
        public int currentAirJump = 0;
        public bool fullHop = true;

        [Header("Lock On")]
        public float softLockonRadius;
        public float lockonRadius;
        public float lockonFudging = 0.1f;
        public LayerMask lockonLayerMask;
        public LayerMask visibilityLayerMask;

        [Header("Enemy Step")]
        public LayerMask enemyStepLayerMask;
        public float enemyStepCheckRadius;

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
        protected override void HandleLockon()
        {
            InputRecordButton lockonButton = InputManager.GetButton((int)EntityInputs.Lockon);

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
            if (LockedOn || InputManager.GetAxis2D((int)EntityInputs.Movement).magnitude < InputConstants.movementMagnitude)
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
                if (c.TryGetComponent(out ITargetable lockonComponent))
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
            Collider[] list = Physics.OverlapSphere(Center, lockonRadius, lockonLayerMask);

            // The direction of the lockon defaults to the forward of the camera.
            Vector3 referenceDirection = GetMovementVector(0, 1);
            // If the movement stick is pointing in a direction, then our lockon should
            // be based on that angle instead.
            Vector2 movementDir = InputManager.GetAxis2D((int)EntityInputs.Movement);
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
                if (c.TryGetComponent(out ITargetable targetLockonComponent))
                {
                    // The target can not be locked on to right now.
                    if (!targetLockonComponent.Targetable)
                    {
                        continue;
                    }
                    //Vector3 targetDistance = (c.transform.position - (transform.position+centerOffset));
                    Vector3 targetDistance = targetLockonComponent.Center - Center;
                    // If we can't see the target, it can not be locked on to.
                    if(Physics.Raycast(Center, targetDistance.normalized, lockonRadius, visibilityLayerMask))
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
            Vector2 movement = InputManager.GetAxis2D((int)EntityInputs.Movement, frame);
            return GetMovementVector(movement.x, movement.y);
        }

        /// <summary>
        /// Translates the given vector based on the look transform's forward.
        /// </summary>
        /// <param name="horizontal">The horizontal axis of the vector.</param>
        /// <param name="vertical">The vertical axis of the vector.</param>
        /// <returns>A direction vector based on the camera's forward.</returns>
        public override Vector3 GetMovementVector(float horizontal, float vertical)
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
        /// <param name="direction">The direction vector.</param>
        /// <returns>A direction vector based on the visual's forward.</returns>
        public override Vector3 GetVisualBasedDirection(Vector3 direction)
        {
            Vector3 vector = visualTransform.TransformDirection(direction);
            return vector;
        }

        /// <summary>
        /// Rotate the visual towards the given direction based on the speed given.
        /// </summary>
        /// <param name="direction">The direction to rotate towards.</param>
        /// <param name="speed">The speed of the rotation.</param>
        public override void RotateVisual(Vector3 direction, float speed)
        {
            Vector3 newDirection = Vector3.RotateTowards(visual.transform.forward, direction, speed, 0.0f);
            visual.transform.rotation = Quaternion.LookRotation(newDirection);
        }

        /// <summary>
        /// Set the visual's rotation to the given direction.
        /// </summary>
        /// <param name="direction">The direction to set the rotation to.</param>
        public override void SetVisualRotation(Vector3 direction)
        {
            visual.transform.rotation = Quaternion.LookRotation(direction);
        }

        /// <summary>
        /// If the entity can currently air jump.
        /// </summary>
        /// <returns>True if the entity can air jump currently.</returns>
        public virtual bool CheckAirJump()
        {
            if (InputManager.GetButton((int)EntityInputs.Jump).firstPress)
            {
                if (currentAirJump < definition.stats.maxAirJumps)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        // Tries to jump cancel if possible.
        /// </summary>
        /// <returns>True if the jump cancel was successful.</returns>
        public virtual bool JumpCancel()
        {
            if (InputManager.GetButton((int)EntityInputs.Jump).firstPress)
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
            if (InputManager.GetButton((int)EntityInputs.Jump, 0, true).firstPress)
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
        }
    }
}