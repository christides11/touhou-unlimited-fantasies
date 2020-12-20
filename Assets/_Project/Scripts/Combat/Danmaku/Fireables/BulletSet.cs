using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class BulletSet : Fireable
    {
        public GameObject bulletPrefab;

        public List<GameObject> bullets = new List<GameObject>();
        public List<DanmakuConfig> bulletsConfig = new List<DanmakuConfig>();

        public List<IDanmakuModifier> modifiers = new List<IDanmakuModifier>();

        public virtual void Update()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].transform.position += bullets[i].transform.forward 
                    * bulletsConfig[i].speed.z.GetValue();
            }
        }

        public override void Fire(DanmakuConfig config)
        {
            GameObject go = GameObject.Instantiate(bulletPrefab, config.position, Quaternion.identity);
            bullets.Add(go);
            bulletsConfig.Add(config);
        }

        public virtual BulletSet AddModifier()
        {
            return this;
        }

        public virtual BulletSet RemoveModifier()
        {
            return this;
        }

        public virtual void ClearModifiers()
        {

        }
    }
}