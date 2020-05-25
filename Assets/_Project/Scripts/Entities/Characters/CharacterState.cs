using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Entities.Characters
{
    public class CharacterState : EntityState
    {
        public virtual CharacterController pc { get { return (CharacterController)controller; } }
    }
}