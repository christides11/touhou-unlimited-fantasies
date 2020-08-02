﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Core
{
    [CreateAssetMenu(fileName = "StageDefiniton", menuName = "TAPI/StageDefinition")]
    public class StageDefinition : ScriptableObject
    {
        public string stageName;
        public string sceneName;
        public bool selectableForGamemodes;
    }
}