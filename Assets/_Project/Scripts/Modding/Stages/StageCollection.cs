using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Modding;

namespace TUF.Core
{
    [CreateAssetMenu(fileName = "StageCollection", menuName = "TAPI/StageCollection")]
    public class StageCollection : ScriptableObject
    {
        public string collectionName;
        public List<GamemodeStageDefinition> stages = new List<GamemodeStageDefinition>();
    }

    [System.Serializable]
    public class GamemodeStageDefinition
    {
        public ModObjectReference gamemode;
        public ModObjectReference stage;

        public override string ToString()
        {
            return $"{gamemode.ToString()} & {stage.ToString()}";
        }
    }
}