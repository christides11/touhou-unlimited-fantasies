using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Core;

namespace TAPI.Combat.Bullets
{
    public class BulletPatternManager : SimObject
    {
        private List<BulletPatternData> patterns = new List<BulletPatternData>();
        // Pattern Index : Bullet List
        public Dictionary<int, List<Bullet>> patternBullets = new Dictionary<int, List<Bullet>>();

        public void Initialize(BulletPattern pattern, Transform position)
        {
            AddPattern(pattern, position);
        }

        public override void SimUpdate()
        {
            base.SimUpdate();
            Tick();
        }

        public void Tick()
        {
            for(int i = 0; i < patterns.Count; i++)
            {
                while (patterns[i].patternPosition < patterns[i].bulletPattern.actions.Count)
                {
                    // If it returns true, we need to wait for a tick.
                    if (patterns[i].bulletPattern.actions[patterns[i].patternPosition].Process(this, i, patterns[i]))
                    {
                        break;
                    }
                    patterns[i].patternPosition++;
                }

                if (patternBullets.ContainsKey(i))
                {
                    for(int j = 0; j < patternBullets[i].Count; j++)
                    {
                        patternBullets[i][j].Tick();
                    }
                }
            }

            if(patterns.Count == 0)
            {
                simObjectManager.DestroyObject(this);
            }
        }

        public void AddPattern(BulletPattern pattern, Transform forwardRelation)
        {
            GameObject patternGO = new GameObject();
            patternGO.transform.SetParent(transform, false);
            patternGO.transform.position = forwardRelation.position;
            patternGO.transform.rotation = forwardRelation.rotation;
            patterns.Add(new BulletPatternData(pattern, patternGO.transform));
        }

        public void CreateBullet(int patternIndex, Bullet bullet)
        {
            if (!patternBullets.ContainsKey(patternIndex))
            {
                patternBullets.Add(patternIndex, new List<Bullet>());
            }

            Bullet b = GameObject.Instantiate(bullet.gameObject, transform, false).GetComponent<Bullet>();
            b.SetSpeed(b.GetForwardBasedSpeed(patterns[patternIndex].currentSpeed));
            patternBullets[patternIndex].Add(b);
        }

        public void Clear()
        {

        }
    }
}