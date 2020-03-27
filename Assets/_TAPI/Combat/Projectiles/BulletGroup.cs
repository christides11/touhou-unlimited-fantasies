using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    [System.Serializable]
    public class BulletGroup
    {
        [System.Serializable]
        public class BulletGroupSpawn {
            public int frame;
            public Vector3 offset;
            public Vector3 rotation;
        }

        public Bullet bullet;
        public BulletDefinition bulletDefinition;
        public List<BulletGroupSpawn> spawns = new List<BulletGroupSpawn>();
    }
}