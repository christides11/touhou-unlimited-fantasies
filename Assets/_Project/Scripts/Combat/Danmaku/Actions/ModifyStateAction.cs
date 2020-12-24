using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class ModifyStateAction : DanmakuAction
    {
        public List<string> bulletSets = new List<string>();

        public DanmakuConfig config;

        public override void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {
            foreach (string d in bulletSets)
            {
                string s = info.id + d;
                for (int j = 0; j < info.bulletSets[s].bulletsConfig.Count; j++) {
                    DanmakuState ds = info.bulletSets[s].bulletsConfig[j];
                    ds.speed += config.speed.GetValue();
                    ds.angularSpeed += config.angularSpeed.GetValue();
                    ds.rotation += config.rotation;
                    info.bulletSets[s].bullets[j].transform.eulerAngles += config.rotation;
                    info.bulletSets[s].bulletsConfig[j] = ds;
                }
            }

            info.NextAction();
        }

        public override void DrawInspector()
        {
#if UNITY_EDITOR

#endif
        }
    }
}