using System.Collections;
using System.Collections.Generic;
using TUF.Combat.Bullets;
using UnityEngine;

namespace TUF.Combat
{
    [CreateAssetMenu(fileName = "AttackDefinition", menuName = "Touhou/Combat/Attack Definition")]
    public class AttackDefinition : CAF.Combat.AttackDefinition
    {
        #region Cancels
        public List<Vector2Int> dashCancelableFrames = new List<Vector2Int>();
        public List<Vector2Int> floatCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> bulletCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> specialCancelFrames = new List<Vector2Int>();
        #endregion

        public new List<BoxGroup> boxGroups;

        public List<BulletPatternGroup> bulletGroups = new List<BulletPatternGroup>();
        public List<AttackFaceLockonWindow> faceLockonTargetWindows = new List<AttackFaceLockonWindow>();
    }
}