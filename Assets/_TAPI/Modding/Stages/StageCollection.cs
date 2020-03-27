using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Modding;

namespace TAPI.Core
{
    [CreateAssetMenu(fileName = "StageCollection", menuName = "TAPI/StageCollection")]
    public class StageCollection : ScriptableObject
    {
        public string collectionName;
        public List<ModStageReference> stages = new List<ModStageReference>();
    }
}