﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Bullets
{
    [System.Serializable]
    public class BulletPatternAction
    {
        public virtual bool Process(BulletPatternManager manager, int group, BulletPatternData data)
        {
            return false;
        }
    }
}