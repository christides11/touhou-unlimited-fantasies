﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    [System.Serializable]
    public class AttackEventDefinition
    {
        public string nickname = "event";
        public bool active;
        public bool onHit;
        public uint startFrame;
        public uint endFrame;
        public AttackEvent attackEvent;
        public AttackEventVariables variables = new AttackEventVariables();
    }
}