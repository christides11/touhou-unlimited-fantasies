using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Modding
{
    [System.Serializable]
    public class ModGamemodeReference : ModObjectReference
    {
        public string gamemodeName;

        public ModGamemodeReference(string modIdentifier, string gameModeName)
        {
            this.modIdentifier = modIdentifier;
            gamemodeName = gameModeName;
        }

        public override string ToString()
        {
            return $"{modIdentifier}/{gamemodeName}";
        }
    }
}