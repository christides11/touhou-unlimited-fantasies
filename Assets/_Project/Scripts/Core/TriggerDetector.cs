using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF
{
    public class TriggerDetector : MonoBehaviour
    {
        public delegate void TriggerEnterAction(Collider other);
        public event TriggerEnterAction TriggerEnter;
        public delegate void TriggerStayAction(Collider other);
        public event TriggerStayAction TriggerStay;
        public delegate void TriggerExitAction(Collider other);
        public event TriggerExitAction TriggerExit;

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
