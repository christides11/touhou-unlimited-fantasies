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
    public class EntityPhysicsManager : MonoBehaviour
    {
        public bool ApplyGravity { get; set; } = true;
        public float GravityScale { get; set; } = 1.0f;
        public float CurrentFallSpeed { get; set; } = 0;

        [SerializeField] protected EntityController controller;

        [Header("Forces")]
        public Vector3 forceMovement;
        public Vector3 forceGravity;
        public Vector3 forceDamage;
        public Vector3 forceInertia;
        public Vector3 forcePushbox;

        [Header("Physics")]
        public float wallCheckDistance = 0.7f;
        public GameObject currentWall;
        public float ceilingCheckDistance = 1.2f;
        public RaycastHit wallRayHit;

        public void HandlePushForce(Collider other)
        {
            Vector3 dir = controller.pushbox.transform.position - other.transform.position;
            forcePushbox = dir * controller.GameManager.gameVars.pushboxForce;
        }

        public virtual void Tick()
        {
            if (ApplyGravity)
            {
                HandleGravity(controller.definition.stats.maxFallSpeed, controller.definition.stats.gravity);
            }
            controller.cc.SetMovement(forceMovement+forcePushbox, forceDamage, forceGravity, forceInertia);
            ApplyInertiaFriction();
            forcePushbox = Vector3.zero;
        }

        public virtual void SetForceDirect(Vector3 movement, Vector3 gravity)
        {
            controller.cc.SetMovement(movement, gravity);
        }

        public virtual Vector3 GetOverallForce()
        {
            return forceMovement + forceGravity + forceDamage;
        }

        public virtual void HandleGravity(float maxFallSpeed, float gravity)
        {
            if (forceGravity.y > -(maxFallSpeed))
            {
                forceGravity.y -= gravity;
                if (forceGravity.y < -(maxFallSpeed))
                {
                    forceGravity.y = -maxFallSpeed;
                }
            }
            else if (forceGravity.y < -(maxFallSpeed))
            {
                forceGravity.y *= 0.97f;
            }
        }

        public virtual void ApplyMovementFriction(float friction = -1)
        {
            if (friction == -1)
            {
                friction = controller.definition.stats.groundFriction;
            }
            Vector3 realFriction = forceMovement.normalized * friction;
            forceMovement.x = ApplyFriction(forceMovement.x, Mathf.Abs(realFriction.x));
            forceMovement.z = ApplyFriction(forceMovement.z, Mathf.Abs(realFriction.z));
        }

        public virtual void ApplyInertiaFriction(float friction = -1)
        {
            if(forceInertia == Vector3.zero)
            {
                return;
            }
            if(friction == -1)
            {
                friction = controller.definition.stats.inertiaFriction;
            }
            Vector3 realFriction = forceInertia.normalized * friction;
            forceInertia.x = ApplyFriction(forceInertia.x, Mathf.Abs(realFriction.x));
            forceInertia.y = ApplyFriction(forceInertia.y, Mathf.Abs(realFriction.y));
            forceInertia.z = ApplyFriction(forceInertia.z, Mathf.Abs(realFriction.z));
        }

        public virtual void ApplyGravityFriction(float friction)
        {
            forceGravity.y = ApplyFriction(forceGravity.y, friction);
        }

        protected virtual float ApplyFriction(float value, float traction)
        {
            if (value > 0)
            {
                value -= traction;
                if (value < 0)
                {
                    value = 0;
                }
            }
            else if (value < 0)
            {
                value += traction;
                if (value > 0)
                {
                    value = 0;
                }
            }
            return value;
        }

        public virtual void ApplyMovement(float accel, float max, float decel, float rotSpeed)
        {
            Vector2 movement = controller.InputManager.GetMovement(0);
            if (movement.magnitude >= InputConstants.movementMagnitude)
            {
                //Translate movment based on "camera."
                Vector3 translatedMovement = controller.lookTransform.TransformDirection(new Vector3(movement.x, 0, movement.y));
                translatedMovement.y = 0;
                translatedMovement *= accel;

                forceMovement += translatedMovement;
                //Limit movement velocity.
                if (forceMovement.magnitude > max * movement.magnitude)
                {
                    forceMovement *= decel;
                }
            }
        }

        /// <summary>
        /// Check if we are on the ground.
        /// </summary>
        public virtual void GroundedCheck()
        {
            controller.IsGrounded = controller.cc.Motor.GroundingStatus.IsStableOnGround;
        }

        /// <summary>
        /// Check if there is something above us.
        /// </summary>
        /// <returns></returns>
        public virtual bool CeilingAbove()
        {
            bool hit = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0),
                Vector2.up, ceilingCheckDistance, controller.GroundedLayerMask);
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
            Vector2 movement = controller.InputManager.GetMovement(0);
            Vector3 translatedMovement = controller.lookTransform.TransformDirection(new Vector3(movement.x, 0, movement.y));
            translatedMovement.y = 0;

            RaycastHit rayHit = new RaycastHit();
            if (translatedMovement.magnitude > InputConstants.movementMagnitude)
            {
                Physics.Raycast(transform.position + new Vector3(0, 1, 0),
                    translatedMovement.normalized, out rayHit, wallCheckDistance, controller.GroundedLayerMask);
            }
            return rayHit;
        }
    }
}
