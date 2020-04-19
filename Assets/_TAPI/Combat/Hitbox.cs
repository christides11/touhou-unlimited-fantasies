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
        [SerializeField] protected GameObject sphereVisual;

        /// <summary>
        /// Deactivates the hitbox.
        /// </summary>
        public virtual void Deactivate()
        {
            coll.enabled = false;
            activated = false;
        }

        /// <summary>
        /// Activate the hitbox.
        /// </summary>
        /// <param name="owner">The owner of this hitbox.</param>
        /// <param name="directionOwner">The transform of the owner.</param>
        /// <param name="hitInfo">The information used for hits.</param>
        /// <param name="ignoreList">The hurtables to ignore.</param>
        public virtual void Activate(GameObject owner, Transform directionOwner, HitInfo hitInfo, List<IHurtable> ignoreList = null)
        {
            this.owner = owner;
            this.directionOwner = directionOwner;
            this.hitInfo = hitInfo;
            coll.enabled = true;
            activated = true;
            this.ignoreList = ignoreList;
        }

        /// <summary>
        /// Reactivates the hitbox, settings it's parameters back to their defaults.
        /// </summary>
        /// <param name="ignoreList"></param>
        public virtual void ReActivate(List<IHurtable> ignoreList = null)
        {
            this.ignoreList = ignoreList;
            coll.enabled = true;
            activated = true;
            hitHurtables.Clear();
            hitHitboxes.Clear();
        }

        /// <summary>
        /// Initializes the hitbox as a rectangle type hitbox.
        /// </summary>
        /// <param name="size">The size of the hitbox.</param>
        /// <param name="rotation">The rotation of the hitbox.</param>
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

        public virtual void InitSphere(float radius)
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            coll = sc;
            sc.radius = radius;


            sphereVisual.transform.localScale = Vector3.one * radius;
            sphereVisual.SetActive(true);
        }

        public override void SimLateUpdate()
        {
            CheckHits();
        }

        /// <summary>
        /// Checks every hurtable and hitbox that we got and properly hurts/disables them.
        /// This should be called every LateUpdate.
        /// </summary>
        public virtual void CheckHits()
        {
            if (hitHurtables.Count > 0)
            {
                for (int i = 0; i < hitHurtables.Count; i++)
                {
                    IHurtable ih = hitHurtables[i].GetComponent<IHurtable>();
                    switch (hitInfo.forceRelation) {
                        case HitForceRelation.ATTACKER:
                            ih.Hurt(directionOwner.position, directionOwner.forward, directionOwner.right, hitInfo);
                            break;
                        case HitForceRelation.HITBOX:
                            ih.Hurt(transform.position, transform.forward, transform.right, hitInfo);
                            break;
                        case HitForceRelation.WORLD:
                            ih.Hurt(transform.position, Vector3.forward, Vector3.right, hitInfo);
                            break;
                    }
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

        /// <summary>
        /// Called every tick for whatever object's are within this hitbox.
        /// Gets all the hitboxes and checks if they should be hurt next LateUpdate.
        /// </summary>
        /// <param name="other">The collider in our hitbox.</param>
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