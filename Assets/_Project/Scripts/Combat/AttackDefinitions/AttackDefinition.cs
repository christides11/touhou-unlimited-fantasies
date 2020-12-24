using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat
{
    [CreateAssetMenu(fileName = "AttackDefinition", menuName = "TUF/Combat/Attack Definition")]
    public class AttackDefinition : CAF.Combat.AttackDefinition
    {
        #region Cancels
        public List<Vector2Int> dashCancelableFrames = new List<Vector2Int>();
        public List<Vector2Int> floatCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> bulletCancelFrames = new List<Vector2Int>();
        public List<Vector2Int> specialCancelFrames = new List<Vector2Int>();
        #endregion

        public List<AttackFaceLockonWindow> faceLockonTargetWindows = new List<AttackFaceLockonWindow>();
    }
}