using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.GameMode
{
    public class GameModeComponent : MonoBehaviour
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