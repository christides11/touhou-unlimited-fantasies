using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Core;

namespace TAPI.Combat.Bullets
{
    public class BulletPatternManager : SimObject
    {
        public List<BulletPatternData> patterns = new List<BulletPatternData>();
        public List<BulletPatternManager> childManagers = new List<BulletPatternManager>();
        // Pattern ID : Bullet List
        public Dictionary<int, List<Bullet>> patternBullets = new Dictionary<int, List<Bullet>>();

        public void Initialize(BulletPattern pattern)
        {
            AddPattern(pattern);
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

                // Tick all the bullets that have been created.
                if (patternBullets.ContainsKey(i))
                {
                    for(int j = 0; j < patternBullets[i].Count; j++)
                    {
                        patternBullets[i][j].Tick();
                    }
                }
            }

            for(int w = 0; w < childManagers.Count; w++)
            {
                childManagers[w].Tick();
            }

            if (CheckForDeletion())
            {
                simObjectManager.DestroyObject(this);
            }
        }

        public bool CheckForDeletion()
        {
            bool childrenDone = true;
            for(int i = 0; i < childManagers.Count; i++)
            {
                if (childManagers[i].CheckForDeletion())
                {
                    simObjectManager.DestroyObject(childManagers[i]);
                }
                else
                {
                    childrenDone = false;
                }
            }

            if (PatternsFinished() && !BulletsExist() && childrenDone)
            {
                return true;
            }
            return false;
        }

        private bool PatternsFinished()
        {
            for(int i = 0; i < patterns.Count; i++)
            {
                if(patterns[i].patternPosition < patterns[i].bulletPattern.actions.Count)
                {
                    return false;
                }
            }
            return true;
        }

        private bool BulletsExist()
        {
            foreach(int key in patternBullets.Keys)
            {
                if(patternBullets[key].Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddPattern(BulletPattern pattern)
        {
            int ID = 0;
            if(patterns.Count > 0)
            {
                ID = patterns[patterns.Count - 1].ID + 1;
            }
            GameObject patternGO = new GameObject();
            patternGO.transform.SetParent(transform, false);
            patternGO.transform.position = transform.position;
            patternGO.transform.rotation = transform.rotation;
            patterns.Add(new BulletPatternData(ID, pattern, patternGO.transform));
        }
        
        public void AddChildPatternManager(int patternIndex, BulletPattern pattern)
        {
            GameObject patternManager = new GameObject();
            patternManager.transform.SetParent(transform, false);
            patternManager.transform.localPosition = patterns[patternIndex].currentOffset;
            BulletPatternManager bpm = patternManager.AddComponent<BulletPatternManager>();
            bpm.Initialize(pattern);
            bpm.Init(simObjectManager);
        }

        /// <summary>
        /// Create a bullet for a given pattern.
        /// </summary>
        /// <param name="patternIndex"></param>
        /// <param name="bullet"></param>
        public void CreateBullet(int patternIndex, Bullet bullet)
        {
            if (!patternBullets.ContainsKey(patterns[patternIndex].ID))
            {
                patternBullets.Add(patterns[patternIndex].ID, new List<Bullet>());
            }

            Bullet b = GameObject.Instantiate(bullet.gameObject, transform, false).GetComponent<Bullet>();
            b.SetPosition(transform.position + patterns[patternIndex].currentOffset);
            b.SetSpeed(patterns[patternIndex].currentSpeed);
            b.SetLocalSpeed(patterns[patternIndex].currentLocalSpeed);
            b.SetAngularSpeed(patterns[patternIndex].currentAngularSpeed);
            b.SetLocalAngularVelocity(patterns[patternIndex].currentLocalAngularSpeed);
            patternBullets[patterns[patternIndex].ID].Add(b);
        }

        /// <summary>
        /// Destroys all bullets for a given pattern.
        /// </summary>
        /// <param name="patternIndex">The pattern index.</param>
        public void ClearBullets(int patternIndex)
        {
            if (patternBullets.ContainsKey(patterns[patternIndex].ID))
            {
                for(int i = 0; i < patternBullets[patterns[patternIndex].ID].Count; i++)
                {
                    Destroy(patternBullets[patterns[patternIndex].ID][i].gameObject);
                }
                patternBullets[patterns[patternIndex].ID].Clear();
            }
        }

        /// <summary>
        /// Destroys all bullets.
        /// </summary>
        public void ClearAllBullets()
        {
            for(int i = 0; i < patterns.Count; i++)
            {
                ClearBullets(i);
            }
        }
    }
}