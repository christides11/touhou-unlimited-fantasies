using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat
{
    [System.Flags]
    public enum EntityTeams
    {
        None = 0,
        Player = 1,
        Enemy = 2
    }
}