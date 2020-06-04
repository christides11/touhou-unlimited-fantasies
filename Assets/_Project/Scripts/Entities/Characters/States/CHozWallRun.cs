using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Entities.Characters.States
{
    public class CHozWallRun : EntityState
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnUpdate()
        {
            if (CheckInterrupt())
            {
                return;
            }
            StateManager.IncrementFrame();
        }

        public override bool CheckInterrupt()
        {
            if(StateManager.CurrentStateFrame > 40)
            {
                StateManager.ChangeState((int)EntityStates.FALL);
                return true;
            }
            return false;
        }
    }
}