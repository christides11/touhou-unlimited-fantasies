using System.Collections;
using System.Collections.Generic;
using TUF.Entities;
using TUF.Entities.Characters;
using TUF.Entities.Shared;
using Touhou.Entities;
using UnityEngine;
using CharacterManager = TUF.Entities.Characters.CharacterManager;

namespace Touhou.Characters
{
    public class DummyController : CharacterManager
    {

        protected override void SetupDefaultStates()
        {
            DummyIdle idle = new DummyIdle();
            StateManager.AddState(idle, (int)EntityStates.IDLE);
            DummyFall fall = new DummyFall();
            StateManager.AddState(fall, (int)EntityStates.FALL);
            EntityFlinch flinch = new EntityFlinch();
            StateManager.AddState(flinch, (int)EntityStates.FLINCH);
            EntityFlinchAir flinchAir = new EntityFlinchAir();
            StateManager.AddState(flinchAir, (int)EntityStates.FLINCH_AIR);
            EntityTumble tumble = new EntityTumble();
            StateManager.AddState(tumble, (int)EntityStates.TUMBLE);

            StateManager.ChangeState((int)EntityStates.FALL);
        }
    }
}
