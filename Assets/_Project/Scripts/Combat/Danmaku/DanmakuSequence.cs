using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [CreateAssetMenu(fileName = "DanmakuSequence", menuName = "TUF/Danmaku/Sequence")]
    [System.Serializable]
    public class DanmakuSequence : ScriptableObject
    {
        [SerializeReference] public List<DanmakuAction> sequence = new List<DanmakuAction>();
    }
}