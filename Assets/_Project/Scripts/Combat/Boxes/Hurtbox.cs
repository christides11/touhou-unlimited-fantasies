using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using UnityEngine;

namespace TUF.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public GameObject Owner { get { return owner; } }
        public IHurtable Hurtable { get { return owner.GetComponent<IHurtable>(); } }

        [SerializeField] protected GameObject owner;
    }
}