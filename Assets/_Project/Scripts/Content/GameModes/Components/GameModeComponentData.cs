using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.GameMode
{
    [System.Serializable]
    public class GameModeComponentData
    {

        public virtual void DrawInspectorUI()
        {
#if UNITY_EDITOR

#endif
        }
    }
}