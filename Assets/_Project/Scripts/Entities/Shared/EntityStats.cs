using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Entities.Shared
{
    [CreateAssetMenu(fileName = "EntityStats", menuName = "Entities/Stats")]
    public class EntityStats : ScriptableObject
    {
        public float height = 2;

        [Header("Ground Movement")]
        public float groundFriction;
        public float maxWalkSpeed;
        public float walkBaseAccel;
        public float walkAcceleration;
        public float walkRotationSpeed;
        public float maxRunSpeed;
        public float runAcceleration;
        public float runRotationSpeed;
        public float dashSpeed;
        public int dashTime;

        [Header("Jump")]
        public int jumpsquat = 5;
        public float fullHopVelocity;
        public float shortHopJumpVelocity;
        public float jumpHorizontalMomentum;

        [Header("Air Jump")]
        public int maxAirJumps = 1;
        public float airJumpVelocity;
        public float airJumpModi = 1.0f;
        public float airJumpCarriedMomentum;
        public float airJumpHorizontalVelo;

        [Header("Air Movement")]
        public float airRotationSpeed;
        public float airFriction;
        public float airAcceleration;
        public float airDeceleration;
        public float maxAirSpeed;
        public float gravity;
        public float maxFallSpeed;

        [Header("Wall Cling")]
        public float wallClingInitFrictionY;
        public float wallClingInitFrictionXZ;
        public float wallClingFrictionXZ;
        public float wallClingGravity;
        public float wallClingMaxFallSpeed;

        [Header("Wall Jump")]
        public int wallJumpTime;
        public float wallJumpYVelo;
        public float wallJumpHVelo;
        public float wallJumpMinAngle = -1;

        [Header("Float")]
        public float floatFriction;
        public float maxFloatSpeed;
        public float floatVerticalSpeed;
        public float floatBaseAccel;
        public float floatAcceleration;
        public float floatRotationSpeed;
        public float floatLockOnRotationSpeed;

        [Header("Other")]
        public float hitstunGravity; //Gravity while in hitstun.
        public float hitstunMaxFallSpeed;
        public float hitstunFrictionXZ;
        public float inertiaFriction;
        public float weight = 1;
    }
}