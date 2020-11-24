using CAF.Combat;
using CAF.Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Combat
{
    public class DetectionBox : SimObject
    {
        public delegate void DetectAction(GameObject hurtable);
        public event DetectAction OnDetect;

        public List<IHurtable> ignoreList = null;
        // THe hurtables hit this frame.
        public List<GameObject> hitHurtables = new List<GameObject>();

        protected GameObject owner;
        protected Transform directionOwner;
        protected bool activated;
        protected Collider coll;

        [SerializeField] protected GameObject boxVisual;
        [SerializeField] protected GameObject sphereVisual;

        public void Initialize(GameObject owner, Vector3 size, Vector3 rotation)
        {
            this.owner = owner;

            transform.rotation = Quaternion.Euler(rotation);
            BoxCollider bc = gameObject.AddComponent<BoxCollider>();
            bc.isTrigger = true;
            coll = bc;
            bc.size = size;
            bc.enabled = false;
            
            boxVisual.transform.localScale = size;
            boxVisual.SetActive(true);
        }

        public void Initialize(GameObject owner, float radius)
        {
            this.owner = owner;

            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            coll = sc;
            sc.radius = radius;
            sc.enabled = false;

            sphereVisual.transform.localScale = Vector3.one * radius;
            sphereVisual.SetActive(true);
        }

        public void Activate(List<IHurtable> ignoreList = null)
        {
            coll.enabled = true;
            activated = true;
            this.ignoreList = ignoreList;
        }

        /// <summary>
        /// Deactivates the detectbox.
        /// </summary>
        public virtual void Deactivate()
        {
            coll.enabled = false;
            activated = false;
        }

        public override void SimLateUpdate()
        {
            CheckDetection();
        }

        public void CheckDetection()
        {
            if (hitHurtables.Count > 0)
            {
                for (int i = 0; i < hitHurtables.Count; i++)
                {
                    IHurtable ih = hitHurtables[i].GetComponent<IHurtable>();
                    if (ignoreList.Contains(ih))
                    {
                        continue;
                    }
                    ignoreList.Add(ih);
                    OnDetect?.Invoke(hitHurtables[i]);
                }
                hitHurtables.Clear();
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (!activated)
            {
                return;
            }

            Hurtbox otherHurtbox = null;
            if (!other.TryGetComponent<Hurtbox>(out otherHurtbox))
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
        }
    }
}