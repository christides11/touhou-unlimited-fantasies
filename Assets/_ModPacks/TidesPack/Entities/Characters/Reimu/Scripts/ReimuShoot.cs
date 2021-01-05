using System.Collections;
using System.Collections.Generic;
using TUF.Entities;
using TUF.Entities.Characters;
using UnityEngine;

namespace TidesPack.Characters.Reimu
{
    public class ReimuShoot : CharacterState
    {

        public override void OnUpdate()
        {


            CheckInterrupt();
        }
    }
}