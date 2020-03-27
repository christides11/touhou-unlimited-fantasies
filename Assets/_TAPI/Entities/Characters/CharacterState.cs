using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Entities.Characters
{
    public class CharacterState : EntityState
    {
        public virtual CharacterController pc { get { return (CharacterController)controller; } }
    }
}