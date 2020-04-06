using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities;
using TAPI.Entities.Characters;
using UnityEngine;

namespace TidesPack.Characters.Reimu
{
    public class ReimuStateTeleport : CharacterState
    {

        public override void OnStart()
        {
            base.OnStart();
            GameManager.current.ConsoleWindow.WriteLine("Reimu teleport.");
        }

        public override void OnUpdate()
        {
            if (!CheckInterrupt())
            {
                StateManager.IncrementFrame();
            }
        }

        public override bool CheckInterrupt()
        {
            if(StateManager.CurrentStateFrame == 25)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                controller.CombatManager.Reset();
                return true;
            }
            return false;
        }
    }
}