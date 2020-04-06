using System.Collections;
using System.Collections.Generic;
using TAPI.Entities.Characters;
using UnityEngine;
using CharacterController = TAPI.Entities.Characters.CharacterController;

namespace TidesPack.Characters.Reimu
{
    public class ReimuController : CharacterController
    {

        protected override void SetupDefaultStates()
        {
            StateManager.AddState(new ReimuStateTeleport(), (int)ReimuStates.SPECIAL_TELEPORT);
            base.SetupDefaultStates();
        }
    }
}