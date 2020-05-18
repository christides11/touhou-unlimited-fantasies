﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    [System.Serializable]
    public class AttackEventDefinition
    {
        public string nickname = "event";
        public bool active = true;
        public bool onHit;
        public int onHitHitboxGroup;
        public bool onDetect;
        public int onDetectHitboxGroup;
        public uint startFrame = 1;
        public uint endFrame = 1;
        public AttackEvent attackEvent;
        public AttackEventVariables variables = new AttackEventVariables();
    }
}