using UnityEngine;

namespace TUF.Core
{
    [CreateAssetMenu(fileName = "StageDefiniton", menuName = "TUF/StageDefinition")]
    public class StageDefinition : ScriptableObject
    {
        public string stageIdentifier;
        public string stageName;
        public string sceneName;
        public bool selectableForGamemodes;
    }
}