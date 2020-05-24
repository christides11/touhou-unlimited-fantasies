using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using UnityEngine;

namespace TAPI.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public GameObject Owner { get { return owner; } }
        public IHurtable Hurtable { get { return owner.GetComponent<IHurtable>(); } }

        [SerializeField] protected GameObject owner;
    }
}