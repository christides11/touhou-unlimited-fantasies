using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public struct DanmakuConfig
    {
        public Vector3 position;
        public Vector3 rotation;
        public Range3 speed;
        public Range3 angularSpeed;
    }
}