using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.GameMode
{
    [System.Serializable]
    public class GameModeComponent
    {
        protected GameModeBase gameMode;

        public virtual void InitComponent(GameModeBase GameMode)
        {
            this.gameMode = GameMode;
        }

        public virtual void Tick()
        {

        }
    }
}