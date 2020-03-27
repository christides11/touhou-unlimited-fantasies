using System.Collections;
using System.Collections.Generic;
using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.Combat
{
    public class AttackEvent : ScriptableObject
    {
        public List<string> intVariables = new List<string>();
        public List<string> floatVariables = new List<string>();
        public List<string> objectVariables = new List<string>();
        public List<string> curveVariables = new List<string>();

        public virtual void Evaluate(uint frame, uint endFrame, EntityAttack attackState, EntityController controller,
            AttackEventVariables variables)
        {

        }
    }
}