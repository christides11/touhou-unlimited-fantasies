using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    [CreateAssetMenu(fileName = "BulletDefinition", menuName = "Combat/Projectiles/Bullet Definition")]
    public class BulletDefinition : ScriptableObject
    {
        [System.Serializable]
        public class ActionDefinition
        {
            public int length;
            public BulletAction action;
        }

        public List<ActionDefinition> actions = new List<ActionDefinition>();
    }
}