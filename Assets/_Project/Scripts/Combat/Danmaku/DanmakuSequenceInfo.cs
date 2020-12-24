using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class DanmakuSequenceInfo
    {
        public DanmakuSequence sequence;
        public DanmakuConfig baseConfig;
        public List<FireableInfo> bulletSets = new List<FireableInfo>();
        public int index;
        public int frame;

        public int currentSet = 0;

        public DanmakuSequenceInfo(DanmakuSequence sequence, DanmakuConfig baseConfig)
        {
            this.sequence = sequence;
            this.baseConfig = baseConfig;
        }

        public virtual void Tick(DanmakuManager manager)
        {
            MoveBullets();
            TickModifiers();
            if (index >= sequence.sequence.Count)
            {
                return;
            }

            sequence.sequence[index].Tick(manager, this);
        }

        private void MoveBullets()
        {
            for (int i = 0; i < bulletSets.Count; i++)
            {
                for (int s = 0; s < bulletSets[i].bullets.Count; s++)
                {
                    FireableInfo fi = bulletSets[i];
                    fi.bullets[s].transform.position +=
                        bulletSets[i].bullets[s].transform.forward * bulletSets[i].bulletsConfig[s].speed.z;

                    fi.bullets[s].transform.Rotate(fi.bulletsConfig[s].angularSpeed, Space.Self);
                }
            }
        }

        private void TickModifiers()
        {
            for(int i = 0; i < bulletSets.Count; i++)
            {
                for(int s = 0; s < bulletSets[i].modifiers.Count; s++)
                {
                    bulletSets[i].modifiers[s].Tick(bulletSets[i]);
                }
            }
        }

        public virtual void NextAction()
        {
            index++;
            frame = 0;
        }

    }
}