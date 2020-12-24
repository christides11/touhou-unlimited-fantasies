using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public struct DanmakuState
    {
        public Transform startPoint;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 speed;
        public Vector3 angularSpeed;
    }
}