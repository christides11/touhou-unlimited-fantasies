using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Modding
{
    [CreateAssetMenu(fileName = "ModObjectLink", menuName = "TUF/Modding/Mod Object Link")]
    public class ModObjectLink : ScriptableObject
    {
        public ModObjectReference reference;
    }
}