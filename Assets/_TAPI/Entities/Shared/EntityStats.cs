using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Shared
{
    [CreateAssetMenu(fileName = "EntityStats", menuName = "Entities/Stats")]
    public class EntityStats : ScriptableObject
    {
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

        [Header("Jumping")]
        public int jumpsquat = 4;
        public float fullHopVelocity;
        public float shortHopJumpVelocity;

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

        [Header("Float")]
        public float floatFriction;
        public float maxFloatSpeed;
        public float floatVerticalSpeed;
        public float floatBaseAccel;
        public float floatAcceleration;
        public float floatRotationSpeed;

        [Header("Other")]
        public float hitstunGravity; //Gravity while in hitstun.
        public float hitstunMaxFallSpeed;
        public float hitstunFrictionXZ;
        public float inertiaFriction;
        public float weight = 1;
    }
}