using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUF.Core;
using CAF.Simulation;

namespace TUF.Combat.Bullets
{
    public class BulletPatternManager : SimObject
    {
        public BulletPatternManagerSettings settings;

        public List<BulletPatternData> patterns = new List<BulletPatternData>();
        public List<BulletPatternManager> childManagers = new List<BulletPatternManager>();
        // Pattern ID : Bullet List
        public Dictionary<int, List<Bullet>> patternBullets = new Dictionary<int, List<Bullet>>();

        public Vector3 bulletSpawnPosition;
        public Vector3 bulletSpawnRotation;

        public void Initialize(BulletPatternManagerSettings settings, BulletPattern pattern, Vector3 bulletSpawnPosition)
        {
            this.bulletSpawnPosition = bulletSpawnPosition;
            this.settings = settings;
            AddPattern(pattern);
        }

        public override void SimUpdate(float deltaTime)
        {
            base.SimUpdate(deltaTime);
            Tick();
        }

        public void Tick()
        {
            if (settings.active)
            {
                for (int i = 0; i < patterns.Count; i++)
                {
                    while (patterns[i].patternPosition < patterns[i].bulletPattern.actions.Count)
                    {
                        if (!patterns[i].active)
                        {
                            break;
                        }
                        // If it returns true, we need to wait for a tick.
                        if (patterns[i].bulletPattern.actions[patterns[i].patternPosition].Process(this, i, patterns[i]))
                        {
                            break;
                        }
                        patterns[i].patternPosition++;
                    }

                    if (!patterns[i].active)
                    {
                        continue;
                    }

                    if (settings.tickBullets)
                    {
                        // Tick all the bullets that have been created.
                        if (patternBullets.ContainsKey(i))
                        {
                            for (int j = 0; j < patternBullets[i].Count; j++)
                            {
                                patternBullets[i][j].Tick();
                                if (patternBullets[i][j].Lifetime == 0)
                                {
                                    Destroy(patternBullets[i][j].gameObject);
                                    patternBullets[i].RemoveAt(j);
                                }
                            }
                        }
                    }
                }
            }

            for(int w = 0; w < childManagers.Count; w++)
            {
                childManagers[w].Tick();
            }

            if (CheckForDeletion())
            {
                if (settings.autoDelete)
                {
                    simObjectManager.DestroyObject(this);
                }
            }
        }

        public bool CheckForDeletion()
        {
            bool childrenDone = true;
            for(int i = 0; i < childManagers.Count; i++)
            {
                if (childManagers[i].CheckForDeletion())
                {
                    if (childManagers[i].settings.autoDelete)
                    {
                        simObjectManager.DestroyObject(childManagers[i]);
                    }
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
            patterns.Add(new BulletPatternData(ID, pattern, transform));
        }
        
        public void AddChildPatternManager(int patternIndex, BulletPattern pattern, BulletPatternManagerSettings settings = null)
        {
            GameObject patternManager = new GameObject();
            patternManager.transform.SetParent(transform, false);
            patternManager.transform.localPosition = patterns[patternIndex].currentPositionOffset;
            patternManager.transform.rotation = Quaternion.Euler(bulletSpawnRotation + patterns[patternIndex].currentRotationOffset);
            BulletPatternManager bpm = patternManager.AddComponent<BulletPatternManager>();
            bpm.bulletSpawnRotation = bulletSpawnRotation + patterns[patternIndex].currentRotationOffset;
            bpm.Initialize((settings == null) ? this.settings : settings, pattern, bulletSpawnPosition);
            bpm.simObjectManager = simObjectManager;
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
            b.SetPosition(bulletSpawnPosition + patterns[patternIndex].currentPositionOffset);
            b.SetRotation(bulletSpawnRotation + patterns[patternIndex].currentRotationOffset);
            b.SetSpeed(patterns[patternIndex].currentSpeed);
            b.SetLocalSpeed(patterns[patternIndex].currentLocalSpeed);
            b.SetAngularSpeed(patterns[patternIndex].currentAngularSpeed);
            b.SetLocalAngularVelocity(patterns[patternIndex].currentLocalAngularSpeed);
            b.SetLifetime(patterns[patternIndex].currentLifetime);
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