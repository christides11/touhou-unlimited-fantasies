using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Entities.Shared
{
    [System.Serializable]
    public class EntityStats
    {
        public float height = 2;

        [Header("Ground Movement")]
        public float groundFriction = 1.2f;
        public float maxWalkSpeed = 8.0f;
        public float walkBaseAccel = 1.0f;
        public float walkAcceleration = 1.0f;
        public float walkRotationSpeed = 0.35f;
        public float maxRunSpeed = 20;
        public float runAcceleration = 2.5f;
        public float runRotationSpeed = 0.97f;
        public float dashSpeed = 20;
        public int dashTime = 20;

        [Header("Jump")]
        public int jumpsquat = 5;
        public float fullHopVelocity = 16;
        public float shortHopJumpVelocity = 13;
        public float jumpHorizontalMomentum = 0;

        [Header("Air Jump")]
        public int maxAirJumps = 1;
        public float airJumpVelocity = 14;
        public float airJumpModi = 1.0f;
        public float airJumpCarriedMomentum = 0.2f;
        public float airJumpHorizontalVelo = 12.0f;

        [Header("Crouch Jump")]
        public float crouchJumpVelocity;
        public float crouchJumpAcceleration;
        public float crouchJumpDeceleration;
        public float crouchJumpMaxAirSeed;

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