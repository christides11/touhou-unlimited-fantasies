using TUF.Core;
using UnityEngine;

namespace TUF.Entities
{
    public class EntityPhysicsManager : CAF.Entities.EntityPhysicsManager
    {
        protected EntityController Controller { get { return (EntityController)controller; } }
        public float CurrentFallSpeed { get; set; } = 0;

        [Header("Physics")]
        public float wallCheckDistance = 0.7f;
        public GameObject currentWall;
        public float ceilingCheckDistance = 1.2f;
        public RaycastHit wallRayHit;

        public override void Tick()
        {
            Controller.cc.SetMovement(forceMovement+forcePushbox, forceDamage, forceGravity);
            forcePushbox = Vector3.zero;
        }

        public void HandlePushForce(Collider other)
        {
            Vector3 dir = Controller.pushbox.transform.position - other.transform.position;
            forcePushbox = dir * Controller.GameManager.gameVars.pushboxForce;
        }

        public override void SetForceDirect(Vector3 movement, Vector3 gravity)
        {
            Controller.cc.SetMovement(movement, gravity);
        }

        public override Vector3 GetOverallForce()
        {
            return forceMovement + forceGravity + forceDamage;
        }

        public virtual void HandleGravity()
        {
            HandleGravity(Controller.definition.stats.maxFallSpeed, 
                Controller.definition.stats.gravity, GravityScale);
        }

        public virtual void HandleGravity(float gravity)
        {
            HandleGravity(Controller.definition.stats.maxFallSpeed, gravity, GravityScale);
        }

        public virtual void HandleGravity(float gravity, float gravityScale)
        {
            HandleGravity(Controller.definition.stats.maxFallSpeed, gravity, gravityScale);
        }

        public override void ApplyMovementFriction(float friction = -1)
        {
            if (friction == -1)
            {
                friction = Controller.definition.stats.groundFriction;
            }
            Vector3 realFriction = forceMovement.normalized * friction;
            forceMovement.x = ApplyFriction(forceMovement.x, Mathf.Abs(realFriction.x));
            forceMovement.z = ApplyFriction(forceMovement.z, Mathf.Abs(realFriction.z));
        }

        public override void ApplyGravityFriction(float friction)
        {
            forceGravity.y = ApplyFriction(forceGravity.y, friction);
        }

        /// <summary>
        /// Applies friction on the given value based on the traction given.
        /// </summary>
        /// <param name="value">The value to apply traction to.</param>
        /// <param name="traction">The traction to apply.</param>
        /// <returns>The new value with the traction applied.</returns>
        protected override float ApplyFriction(float value, float traction)
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

        /// <summary>
        /// Create a force based on the parameters given and
        /// adds it to our movement force.
        /// </summary>
        /// <param name="accel">How fast the entity accelerates in the movement direction.</param>
        /// <param name="max">The max magnitude of our movement force.</param>
        /// <param name="decel">How much the entity decelerates when moving faster than the max magnitude.
        /// 1.0 = doesn't decelerate, 0.0 = force set to 0.</param>
        public override void ApplyMovement(float accel, float max, float decel)
        {
            Vector2 movement = Controller.InputManager.GetAxis2D((int)EntityInputs.Movement);
            if (movement.magnitude >= InputConstants.movementMagnitude)
            {
                //Translate movment based on "camera."
                Vector3 translatedMovement = Controller.GetMovementVector(movement.x, movement.y);
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
        public override void CheckIfGrounded()
        {
            Controller.IsGrounded = Controller.cc.Motor.GroundingStatus.IsStableOnGround;
        }

        /// <summary>
        /// Check if there is something above us.
        /// </summary>
        /// <returns></returns>
        public virtual bool CeilingAbove()
        {
            bool hit = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0),
                Vector2.up, ceilingCheckDistance, Controller.GroundedLayerMask);
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
            Vector3 translatedMovement = Controller.GetMovementVector();
            translatedMovement.y = 0;

            wallRayHit = new RaycastHit();
            if (translatedMovement.magnitude > InputConstants.movementMagnitude)
            {
                Physics.Raycast(transform.position + new Vector3(0, 1, 0),
                    translatedMovement.normalized, out wallRayHit, wallCheckDistance, Controller.GroundedLayerMask);
            }
            return wallRayHit;
        }
    }
}
