﻿using System.Collections;
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

        public void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(other);
        }

        public void OnTriggerStay(Collider other)
        {
            TriggerStay?.Invoke(other);
        }

        public void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke(other);   
        }
    }
}