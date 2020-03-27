using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    [System.Serializable]
    public class HitInfo
    {
        public bool airOnly;
        public bool groundOnly;

        public bool continuousHit;
        public int spaceBetweenHits;
        public bool breakArmor;
        public bool unblockable;
        public bool knockdown;
        public bool groundBounces;
        public float groundBounceForce;
        public bool wallBounces;
        public float wallBounceForce;
        public float damageOnHit;
        public float damageOnBlock;
        public bool hitKills = true;
        public ushort attackerHitstop;
        public ushort hitstop;
        public ushort hitstun;

        public bool opponentResetXForce = true;
        public bool opponentResetYForce = true;
        public Vector3 opponentForceDir = Vector3.forward;
        public float opponentForceMagnitude = 1;
        public bool causesTumble;

        public HitInfo()
        {

        }

        public HitInfo(HitInfo other)
        {
            airOnly = other.airOnly;
            groundOnly = other.groundOnly;

            continuousHit = other.continuousHit;
            spaceBetweenHits = other.spaceBetweenHits;
            breakArmor = other.breakArmor;
            unblockable = other.unblockable;
            knockdown = other.knockdown;

            groundBounces = other.groundBounces;
            groundBounceForce = other.groundBounceForce;
            wallBounces = other.wallBounces;
            wallBounceForce = other.wallBounceForce;

            damageOnHit = other.damageOnHit;
            damageOnBlock = other.damageOnBlock;
            hitKills = other.hitKills;
            attackerHitstop = other.attackerHitstop;
            hitstop = other.hitstop;
            hitstun = other.hitstun;

            opponentResetXForce = other.opponentResetXForce;
            opponentResetYForce = other.opponentResetYForce;
            opponentForceDir = other.opponentForceDir;
            opponentForceMagnitude = other.opponentForceMagnitude;
            causesTumble = other.causesTumble;
        }
    }
}