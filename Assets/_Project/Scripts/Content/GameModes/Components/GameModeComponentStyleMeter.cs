using UnityEngine;

namespace TUF.GameMode
{
    [System.Serializable]
    public class GameModeComponentStyleMeter : GameModeComponentBase
    {
        [SerializeField] protected float styleDepletionTime = 1.5f;

        public override void InitComponent(GameModeBase GameMode)
        {
            base.InitComponent(GameMode);
        }
    }
}