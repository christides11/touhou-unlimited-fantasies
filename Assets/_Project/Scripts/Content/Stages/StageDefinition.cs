using UnityEngine;

namespace TUF.Core
{
    [CreateAssetMenu(fileName = "StageDefiniton", menuName = "TUF/Content/Stages/StageDefinition")]
    public class StageDefinition : ScriptableObject
    {
        public string stageIdentifier;
        public string stageName;
        public string[] sceneNames;
        public bool selectableForGamemodes;
    }
}