using System;
using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Combat
{
    public class DetectionBox : Hitbox
    {
        public delegate void DetectAction(HitInfo hitInfo);
        public event DetectAction OnDetect;

        public bool detected = false;

        public override void SimLateUpdate()
        {
            CheckDetection();
        }

        private void CheckDetection()
        {
            if (hitHurtables.Count > 0)
            {
                OnDetect?.Invoke(hitInfo);
                detected = true;
                Deactivate();
            }
        }

        protected override void OnTriggerStay(Collider other)
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