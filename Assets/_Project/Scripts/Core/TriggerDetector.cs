using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF
{
    public class TriggerDetector : MonoBehaviour
    {
        public delegate void TriggerAction(Collider other);
        public event TriggerAction TriggerEnter;
        public event TriggerAction TriggerStay;
        public event TriggerAction TriggerExit;

        public Collider Collider { get { return coll; } }

        [SerializeField] private Collider coll;
        [SerializeField] private LayerMask layerMask;

        public void OnTriggerEnter(Collider other)
        {
            if (CheckLayer(other))
            {
                TriggerEnter?.Invoke(other);
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (CheckLayer(other))
            {
                TriggerStay?.Invoke(other);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (CheckLayer(other))
            {
                TriggerExit?.Invoke(other);
            }
        }

        private bool CheckLayer(Collider other)
        {
            return layerMask == (layerMask | (1 << other.gameObject.layer));
        }
    }
}
