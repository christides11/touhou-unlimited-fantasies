using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat
{
    [CreateAssetMenu(fileName = "AttackDefinition", menuName = "Combat/Attack")]
    public class AttackSO : ScriptableObject
    {
        #region General
        public string attackName;
        public string description;
        public int stateOverride = -1;
        public int length; //In frames
        public bool groundAble;
        public bool airAble;
        public float heightRestriction;
        public bool possibleGround;
        public bool modifiesInertia = true;
        public float inertiaModifer = 0.0f;
        public bool carriesInertia = false;
        public float carriedInertia = 1.0f;
        public List<int> chargeFrames = new List<int>();
        public int chargeLength;
        #endregion

        #region Animation
        public AnimationClip animationGround;
        public AnimationClip animationAir;
        public WrapMode wrapMode;
        #endregion

        #region Cancels
        public List<Vector2Int> jumpCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> landCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> dashCancelableFrames = new List<Vector2Int>();
        public List<Vector2Int> floatCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> attackCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> bulletCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> specialCancelFrames = new List<Vector2Int>();
        #endregion

        #region Hitboxes
        public List<HitboxGroup> hitboxGroups = new List<HitboxGroup>();
        #endregion

        public List<AttackFaceLockonWindow> faceLockonTargetWindows = new List<AttackFaceLockonWindow>();

        public List<BulletGroup> bulletGroups = new List<BulletGroup>();

        public List<AttackEventDefinition> events = new List<AttackEventDefinition>();
    }
}