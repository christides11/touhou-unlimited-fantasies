using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Core
{
    public class SimObject : MonoBehaviour
    {
        private SimObjectManager simObjectManager;

        public virtual void Init(SimObjectManager simObjectManager)
        {
            this.simObjectManager = simObjectManager;
        }

        protected virtual void Awake()
        {
            SimAwake();
        }

        protected virtual void Start()
        {
            SimStart();
        }

        public virtual void SimAwake()
        {
        }

        public virtual void SimStart()
        {
        }

        public virtual void SimUpdate()
        {
        }

        public virtual void SimLateUpdate()
        {
        }
    }
}