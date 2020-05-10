using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Modding;
using UnityEngine;

namespace TAPI.Combat
{
    [System.Serializable]
    public class HitboxGroup
    {
        public int ID;
        public int activeFramesStart;
        public int activeFramesEnd;
        public HitboxGroupType hitGroupType;
        public List<HitboxDefinition> hitboxes = new List<HitboxDefinition>();
        public CameraShakeDefinition cameraShake;
        public ModObjectLink hitSound;
        public bool attachToEntity = true;

        [SerializeField] public HitInfo hitInfo = new HitInfo();
        
        #region Throw
        public AttackSO throwConfirm;
        #endregion

        public HitboxGroup()
        {

        }

        public HitboxGroup(HitboxGroup other)
        {
            ID = other.ID;
            activeFramesStart = other.activeFramesStart;
            activeFramesEnd = other.activeFramesEnd;
            hitGroupType = other.hitGroupType;
            hitboxes = new List<HitboxDefinition>(other.hitboxes);
            attachToEntity = other.attachToEntity;
            hitInfo = new HitInfo(other.hitInfo);
            throwConfirm = other.throwConfirm;
        }
    }
}
