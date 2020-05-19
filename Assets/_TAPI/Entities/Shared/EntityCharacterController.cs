using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

namespace TAPI.Entities
{
    public class EntityCharacterController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor;

        [Header("Movement Forces")]
        public Vector3 gravity;

        [Header("Stable Movement")]
        public float stableMovementSharpness = 15f;
        public float orientationSharpness = 10f;

        [Header("Air Movement")]
        public float airMovementSharpness = 15f;
        public float MaxAirMoveSpeed = 15f;

        [Header("Misc")]
        public List<Collider> IgnoredColliders = new List<Collider>();

        private Vector3 moveVector;

        void Awake()
        {
            // Assign the characterController to the motor
            Motor.CharacterController = this;
        }

        public void SetMovement(Vector3 force, Vector3 forceGravity)
        {
            moveVector = force;
            gravity = forceGravity;
        }

        public void SetMovement(Vector3 forceMovement, Vector3 forceDamage, Vector3 forceGravity)
        {
            gravity = forceGravity;

            moveVector = forceMovement + forceDamage;

            if (gravity.y > 0.0f)
            {
                Motor.ForceUnground(0.1f);
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {

        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // Handle landing and leaving ground
            if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLanded();
            }
            else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLeaveStableGround();
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                // Ground Movement
                float currentVelocityMagnitude = currentVelocity.magnitude;

                Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;
                if (currentVelocityMagnitude > 0f && Motor.GroundingStatus.SnappingPrevented)
                {
                    // Take the normal from where we're coming from
                    Vector3 groundPointToCharacter = Motor.TransientPosition - Motor.GroundingStatus.GroundPoint;
                    if (Vector3.Dot(currentVelocity, groundPointToCharacter) >= 0f)
                    {
                        effectiveGroundNormal = Motor.GroundingStatus.OuterGroundNormal;
                    }
                    else
                    {
                        effectiveGroundNormal = Motor.GroundingStatus.InnerGroundNormal;
                    }
                }

                // Reorient velocity on slope
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(moveVector.normalized, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * moveVector.magnitude;
                Vector3 targetMovementVelocity = reorientedInput;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-stableMovementSharpness * deltaTime));
            }
            else
            {
                // Air Movement
                Vector3 targetMovementVelocity = moveVector;

                // Prevent air-climbing sloped walls
                if (Motor.GroundingStatus.FoundAnyGround)
                {
                    if (Vector3.Dot(targetMovementVelocity, targetMovementVelocity - currentVelocity) > 0f)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }
                }

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity + gravity, 1f - Mathf.Exp(-airMovementSharpness * deltaTime));
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (IgnoredColliders.Count == 0)
            {
                return true;
            }

            if (IgnoredColliders.Contains(coll))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {

        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {

        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            Rigidbody r = hitCollider.attachedRigidbody;
            if (r)
            {
                Vector3 relativeVel = Vector3.Project(r.velocity, hitNormal) - Vector3.Project(Motor.Velocity, hitNormal);
            }
        }

        protected void OnLanded()
        {
        }

        protected void OnLeaveStableGround()
        {
        }
    }
}