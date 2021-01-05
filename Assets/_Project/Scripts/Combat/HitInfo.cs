using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat
{
    [System.Serializable]
    public class HitInfo : CAF.Combat.HitInfo
    {
        public int attackerPowerGain;
        public int attackerPowerGainOnHit;

        public override void DrawInspectorHitInfo()
        {
#if UNITY_EDITOR
            base.DrawInspectorHitInfo();

            DrawHitPowerOptions();
            EditorGUILayout.Space();
#endif
        }

        private void DrawHitPowerOptions()
        {
#if UNITY_EDITOR
            EditorGUILayout.LabelField("POWER", EditorStyles.boldLabel);

            attackerPowerGain = EditorGUILayout.IntField("Power Gain", attackerPowerGain);
            attackerPowerGainOnHit = EditorGUILayout.IntField("Power Gain (On Hit)", attackerPowerGainOnHit);
#endif
        }
    }
}