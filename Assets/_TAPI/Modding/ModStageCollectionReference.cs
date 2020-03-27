using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Modding
{
    public class ModStageCollectionReference : ModObjectReference
    {
        public string stageCollectionName;

        public ModStageCollectionReference(string modIdentifier, string stageCollectionName)
        {
            this.modIdentifier = modIdentifier;
            this.stageCollectionName = stageCollectionName;
        }
    }
}