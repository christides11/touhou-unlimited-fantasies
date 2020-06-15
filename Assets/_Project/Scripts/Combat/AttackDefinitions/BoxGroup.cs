using TUF.Core;
using TUF.Modding;

namespace TUF.Combat
{
    [System.Serializable]
    public class BoxGroup : CAF.Combat.BoxGroup
    {
        public CameraShakeDefinition cameraShake;
        public ModObjectLink hitSound;

        public BoxGroup()
        {
        }

        public BoxGroup(TUF.Combat.BoxGroup other) : base(other)
        {

        }
    }
}
