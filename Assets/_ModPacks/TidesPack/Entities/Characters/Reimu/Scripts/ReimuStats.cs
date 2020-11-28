using TUF.Entities.Characters;
using UnityEngine;

namespace TidesPack.Characters.Reimu
{
    public class ReimuStats : CharacterStats
    {
        [Header("Teleport")]
        public float teleportNoTargetForwardDist;
        public float teleportNoTargetUpDist;
        public float teleportTargetForwardDist;
        public float teleportTargetUpDist;
        public float teleportUpwardForce;
        public float teleportForwardForce;
    }
}