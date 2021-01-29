using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TUF.Core
{
    [CreateAssetMenu(fileName = "LocalStageDefiniton", menuName = "TUF/Content/Stages/LocalStageDefinition")]
    public class LocalStageDefinition : StageDefinition
    {
        public AssetReference[] sceneRefs;
    }
}