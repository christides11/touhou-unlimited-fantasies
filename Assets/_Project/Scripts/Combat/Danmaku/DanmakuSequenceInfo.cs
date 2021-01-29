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
        public Dictionary<string, FireableInfo> bulletSets = new Dictionary<string, FireableInfo>();
        public int index;
        public int frame;

        public int currentSet = 0;

        public string id;

        public EntityTeams team;
        public HitInfo hitInfo;

        public DanmakuSequenceInfo(DanmakuSequence sequence, DanmakuConfig baseConfig, EntityTeams team, HitInfo hitInfo, string id = "")
        {
            this.sequence = sequence;
            this.baseConfig = baseConfig;
            this.id = id;
            this.team = team;
            this.hitInfo = hitInfo;
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
            foreach (FireableInfo fi in bulletSets.Values)
            {
                for (int s = 0; s < fi.bullets.Count; s++)
                {
                    if (!fi.bullets[s])
                    {
                        fi.bullets.RemoveAt(s);
                        continue;
                    }
                    fi.bullets[s].transform.position +=
                        fi.bullets[s].transform.forward * fi.bulletsConfig[s].speed.z;

                    fi.bullets[s].transform.Rotate(fi.bulletsConfig[s].angularSpeed, Space.Self);
                    fi.bullets[s].GetComponent<Bullet>().Tick();
                }
            }
        }

        private void TickModifiers()
        {
            foreach (FireableInfo fi in bulletSets.Values)
            {
                for (int s = 0; s < fi.modifiers.Count; s++)
                {
                    fi.modifiers[s].Tick(fi);
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