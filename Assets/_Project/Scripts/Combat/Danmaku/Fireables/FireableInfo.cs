using System.Collections.Generic;
using TUF.Combat.Danmaku.Modifiers;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class FireableInfo
    {
        public List<GameObject> bullets = new List<GameObject>();
        public List<DanmakuState> bulletsConfig = new List<DanmakuState>();
        public List<DanmakuModifier> modifiers = new List<DanmakuModifier>();
    }
}