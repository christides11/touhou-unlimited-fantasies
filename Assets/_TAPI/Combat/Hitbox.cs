using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Combat
{
    public class Hitbox : SimObject
    {
        public delegate void HurtAction(HitInfo hitInfo);
        public event HurtAction OnHurt;

        protected List<IHurtable> ignoreList = null;
        public List<GameObject> hitHurtables = new List<GameObject>();
        public List<Hitbox> hitHitboxes = new List<Hitbox>();

        protected GameObject owner;
        protected Transform directionOwner;
        protected bool activated;
        protected Collider coll;
        public HitInfo hitInfo;

        [SerializeField] protected GameObject boxVisual;
        [SerializeField] protected GameObject circleVisual;

        public virtual void Deactivate()
        {
            coll.enabled = false;
            activated = false;
        }

        public virtual void Activate(GameObject owner, Transform directionOwner, HitInfo hitInfo, List<IHurtable> ignoreList = null)
        {
            this.owner = owner;
            this.directionOwner = directionOwner;
            this.hitInfo = hitInfo;
            coll.enabled = true;
            activated = true;
            this.ignoreList = ignoreList;
        }

        public virtual void ReActivate(List<IHurtable> ignoreList = null)
        {
            this.ignoreList = ignoreList;
            coll.enabled = true;
            activated = true;
            hitHurtables.Clear();
            hitHitboxes.Clear();
        }

        public virtual void InitRectangle(Vector3 size, Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation);

            BoxCollider bc = gameObject.AddComponent<BoxCollider>();
            bc.isTrigger = true;
            coll = bc;
            bc.size = size;

            boxVisual.transform.localScale = size;
            boxVisual.SetActive(true);
        }

        public override void SimLateUpdate()
        {
            CheckHits();
        }

        public virtual void CheckHits()
        {
            if (hitHurtables.Count > 0)
            {
                for(int i = 0; i < hitHurtables.Count; i++)
                {
                    IHurtable ih = hitHurtables[i].GetComponent<IHurtable>();
                    ih.Hurt(directionOwner.forward, directionOwner.right, hitInfo);
                    OnHurt?.Invoke(hitInfo);
                    ignoreList.Add(ih);
                }
            }

            if (hitHitboxes.Count > 0)
            {
                for(int i = 0; i < hitHitboxes.Count; i++)
                {
                    hitHitboxes[i].Deactivate();
                }
                Deactivate();
            }

            hitHurtables.Clear();
            hitHitboxes.Clear();
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (!activated)
            {
                return;
            }

            Hurtbox otherHurtbox = null;
            Hitbox otherHitbox = null;
            if (!other.TryGetComponent<Hurtbox>(out otherHurtbox)
                && !other.TryGetComponent<Hitbox>(out otherHitbox))
            {
                return;
            }

            if (otherHurtbox != null)
            {
                if (!hitHurtables.Contains(otherHurtbox.Owner)
                    && (ignoreList == null || !ignoreList.Contains(otherHurtbox.Hurtable)))
                {
                    hitHurtables.Add(otherHurtbox.Owner);
                }
            }

            if (otherHitbox != null)
            {
                if (!hitHitboxes.Contains(otherHitbox))
                {
                    if (otherHitbox.activated)
                    {
                        hitHitboxes.Add(otherHitbox);
                    }
                }
            }
        }
    }
}