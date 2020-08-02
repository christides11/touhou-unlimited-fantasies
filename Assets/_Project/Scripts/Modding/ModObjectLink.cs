﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Modding
{
    [CreateAssetMenu(fileName = "ModObjectLink", menuName = "TAPI/Mod Object Link")]
    public class ModObjectLink : ScriptableObject
    {
        public ModObjectReference reference;
    }
}