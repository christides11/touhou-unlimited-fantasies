using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlyAttribute = TUF.Core.ReadOnlyAttribute;

namespace TUF.GameMode
{
    [System.Serializable]
    public class GameModeComponentBase
    {
        [SerializeField] [ReadOnly] protected GameModeBase gameMode;

        public virtual void InitComponent(GameModeBase GameMode)
        {
            this.gameMode = GameMode;
        }

        public virtual void Tick()
        {

        } 

        public virtual GameModeComponentData GetComponentData()
        {
            return null;
        }

        public virtual void SetComponentData(GameModeComponentData componentData)
        {

        }
    }
}