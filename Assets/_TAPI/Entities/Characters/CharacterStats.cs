using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Entities.Shared;

namespace TAPI.Entities.Characters
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Entities/Characters/Stats")]
    public class CharacterStats : EntityStats
    {
        [Header("Air Dash")]
        public float airDashVelo;
        public int airDashHoldVelo; //How long to hold the init velo for.
        public int airDashLength;
        public float airDashFriction;

        [Header("Float Dodge")]
        public float floatDodgeVelo;
        public int floatDodgeHoldVelo;
        public int floatDodgeLength;
        public float floatDodgeFriction;

        [Header("Float Dash")]
        public float floatDashMaxSpeed;
        public float floatDashBaseAccel;
        public float floatDashAcceleration;
    }
}