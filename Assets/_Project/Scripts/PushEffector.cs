using System.Collections;
using System.Collections.Generic;
using TUF.Entities;
using UnityEngine;

namespace TUF
{
    public class PushEffector : MonoBehaviour
    {
        public enum TriggerType
        {
            OnEnter, OnStay
        }

        public enum ForceType
        {
            Add, Set
        }

        public enum ForceDirection
        {
            World, Local, Other
        }

        public TriggerType triggerType;
        public ForceType forceType;
        public ForceDirection forceRelation;
        public Vector3 force;
        public float maxMagnitude = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (triggerType != TriggerType.OnEnter)
            {
                return;
            }
            HandleForce(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (triggerType != TriggerType.OnStay)
            {
                return;
            }
            HandleForce(other);
        }

        private void HandleForce(Collider other)
        {
            EntityPhysicsManager epm = null;
            other.TryGetComponent<EntityPhysicsManager>(out epm);
            if (epm == null)
            {
                return;
            }

            Vector3 temp = epm.forceGravity + epm.forceMovement;
            switch (forceType)
            {
                case ForceType.Add:
                    temp += CreateForce(other.GetComponent<EntityManager>());
                    break;
                case ForceType.Set:
                    Vector3 setTemp = CreateForce(other.GetComponent<EntityManager>());
                    break;
            }

            if (maxMagnitude > 0 && temp.magnitude > maxMagnitude)
            {
                temp = temp.normalized * maxMagnitude;
            }

            epm.forceGravity.y = temp.y;
            temp.y = 0;
            epm.forceMovement = temp;
        }

        protected virtual Vector3 CreateForce(EntityManager em)
        {
            Vector3 f = force;

            switch (forceRelation)
            {
                case ForceDirection.Local:
                    f = (transform.forward * f.z)
                        + (transform.right * f.x)
                        + (transform.up * f.y);
                    break;
                case ForceDirection.Other:
                    f = (em.visualTransform.forward * f.z)
                        + (em.visualTransform.right * f.x)
                        + (em.visualTransform.up * f.y);
                    break;
            }

            return f;
        }
    }
}