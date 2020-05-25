using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TUF.Inputs;

namespace TUF.Core
{
    [System.Serializable]
    public class CameraShakeDefinition
    {
        public float amplitude;
        public float frequency;
        public float holdLength;
        public float fadeLength;
        public float amplitudeDecay;
        public float frequencyDecay;
    }
}
