using TUF.Entities.Characters;
using UnityEngine;

namespace TidesPack.Characters.Reimu
{
    [System.Serializable]
    public class ReimuStats : CharacterStats
    {
        [Header("Float Move")]
        public int floatRushLength = 30;
        public float floatRushSpeed = 5;
        
        [Header("Teleport")]
        public float teleportNoTargetForwardDist;
        public float teleportNoTargetUpDist;
        public float teleportTargetForwardDist;
        public float teleportTargetUpDist;
        public float teleportUpwardForce;
        public float teleportForwardForce;
    }
}