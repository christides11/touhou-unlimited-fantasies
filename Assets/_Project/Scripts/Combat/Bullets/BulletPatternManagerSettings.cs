using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    public class BulletPatternManagerSettings
    {
        public bool active = true;
        public bool autoDelete = true;
        public bool tickBullets = true;
        public bool disableLooping = false;

        public BulletPatternManagerSettings()
        {

        }
    }
}