using CAF.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Combat.Danmaku
{
    public class Bullet : MonoBehaviour
    {
        Hitbox3D hitbox;

        public void Tick()
        {
            if (hitbox)
            {
                hitbox.CheckHits();
            }
        }

        public void CreateHitbox(GameObject hitboxOwner, int team, BoxDefinition boxDefinition, HitInfo hitInfo)
        {
            hitbox = InstantiateHitbox(boxDefinition);
            hitbox.Initialize(hitboxOwner, transform, team, BoxShapes.Rectangle, hitInfo, boxDefinition, new List<IHurtable>());
            hitbox.OnHurt += DestroyBullet;
            hitbox.Activate();
        }

        private void DestroyBullet(GameObject hurtableHit, HitInfoBase hitInfo)
        {
            Debug.Log($"{gameObject.name} Hit {hurtableHit.name}.");
        }

        protected Hitbox3D InstantiateHitbox(BoxDefinition boxDefinition)
        {
            GameManager gm = GameManager.current;

            Vector3 position = transform.position
                + transform.forward * boxDefinition.offset.z
                + transform.right * boxDefinition.offset.x
                + transform.up * boxDefinition.offset.y;

            Vector3 rotation = transform.eulerAngles + boxDefinition.rotation;

            GameObject hitbox = GameObject.Instantiate(gm.gameVariables.combat.hitbox, position, Quaternion.Euler(rotation));
            hitbox.transform.SetParent(transform, true);
            return hitbox.GetComponent<Hitbox3D>();
        }
    }
}