using TAPI.Core;
using TAPI.Modding;

namespace TAPI.Combat
{
    [System.Serializable]
    public class BoxGroup : CAF.Combat.BoxGroup
    {
        public CameraShakeDefinition cameraShake;
        public ModObjectLink hitSound;
    }
}
