using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Modding;

namespace TUF.Core
{
    [CreateAssetMenu(fileName = "StageCollection", menuName = "TUF/Content/Stages/StageCollection")]
    public class StageCollection : ScriptableObject
    {
        public string collectionID;
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