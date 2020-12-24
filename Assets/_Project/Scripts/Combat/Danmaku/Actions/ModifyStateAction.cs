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
        public int bulletSetIndex = -1;

        public DanmakuConfig config;

        public override void Tick(DanmakuManager danmakuManager, DanmakuSequenceInfo info)
        {
            int min = bulletSetIndex;
            int max = bulletSetIndex + 1;
            if (bulletSetIndex == -1)
            {
                min = 0;
                max = info.bulletSets.Count;
            }

            for (int i = min; i < max; i++)
            {
                for (int j = 0; j < info.bulletSets[i].bulletsConfig.Count; j++) {
                    DanmakuState ds = info.bulletSets[i].bulletsConfig[j];
                    ds.speed += config.speed.GetValue();
                    ds.angularSpeed += config.angularSpeed.GetValue();
                    ds.rotation += config.rotation;
                    info.bulletSets[i].bullets[j].transform.eulerAngles += config.rotation;
                    info.bulletSets[i].bulletsConfig[j] = ds;
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