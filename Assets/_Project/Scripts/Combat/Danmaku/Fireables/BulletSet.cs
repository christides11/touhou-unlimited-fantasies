using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Danmaku
{
    [System.Serializable]
    public class BulletSet : Fireable
    {
        public GameObject bulletPrefab;

        public override void Fire(FireableInfo fireableInfo, DanmakuConfig config)
        {
            GameObject go = GameObject.Instantiate(bulletPrefab, config.position, Quaternion.Euler(config.rotation));
            go.GetComponent<Bullet>().CreateHitbox(fireableInfo.hitboxOwner, fireableInfo.team, fireableInfo.boxDefinition, fireableInfo.hitInfo);
            fireableInfo.bullets.Add(go);
            fireableInfo.bulletsConfig.Add(config.CreateState());
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

        public override void DrawInspector()
        {
#if UNITY_EDITOR
            bulletPrefab = (GameObject)EditorGUILayout.ObjectField("Bullet Prefab", bulletPrefab, typeof(GameObject), false);
#endif
        }
    }
}