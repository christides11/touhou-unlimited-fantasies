using TUF.Core;
using TUF.Modding;

namespace TUF.Combat
{
    [System.Serializable]
    public class BoxGroup : CAF.Combat.BoxGroup
    {
        public CameraShakeDefinition cameraShake;
        public ModObjectLink hitSound;
    }
}
