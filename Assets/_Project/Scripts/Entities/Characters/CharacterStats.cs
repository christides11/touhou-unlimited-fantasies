using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Entities.Shared;

namespace TUF.Entities.Characters
{
    [System.Serializable]
    public class CharacterStats : EntityStats
    {
        [Header("Air Dash")]
        public int maxAirDashes = 1;
        public float airDashVelo;
        public int airDashHoldVelo; //How long to hold the init velo for.
        public int airDashLength;
        public float airDashFriction;
        public float airDashGravityFriction;
        public float airDashGravityInitMulti;

        [Header("Float Dodge")]
        public float floatDodgeVelo;
        public int floatDodgeHoldVelo;
        public int floatDodgeLength;
        public float floatDodgeFriction;

        [Header("Float Dash")]
        public float floatDashMaxSpeed;
        public float floatDashBaseAccel;
        public float floatDashAcceleration;

        [Header("Wall")]
        public int wallRunTime = 60;
        public float wallRunVerticalSpeed;
        public float wallRunHorizontalSpeed;
        public float wallRunHorizontalChainMulti;
        public float wallRunHorizontalChainMaxMulti;

        [Header("Slide")]
        public float slideInitialSpeed;
        public int slideHoldFrames;
        public float slideSlopeBaseSpeed;
        public float slideSpeedPerAngle;
        public float slideForceTransitionTime;

        [Header("Crouch")]
        public float crouchHeight = 1;

        [Header("Ledge Jump")]
        public float ledgeJumpMaxSpeed;
        public float ledgeJumpYForce;
        public float ledgeJumpMoveForce;
        public float ledgeJumpAccel;
    }
}