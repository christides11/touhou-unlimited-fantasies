using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public struct DanmakuConfig
    {
        public Transform startPoint;
        public Vector3 position;
        public Vector3 rotation;
        public Range3 speed;
        public Range3 angularSpeed;

        /// <summary>
        /// Creates an state based on the config.
        /// </summary>
        /// <returns>a sampled state from the config's state space.</returns>
        public DanmakuState CreateState()
        {
            return new DanmakuState
            {
                position = position,
                rotation = rotation,
                speed = speed.GetValue(),
                angularSpeed = angularSpeed.GetValue()
            };
        }
    }
}