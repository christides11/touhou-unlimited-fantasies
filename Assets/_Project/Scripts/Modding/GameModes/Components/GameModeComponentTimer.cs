using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlyAttribute = TUF.Core.ReadOnlyAttribute;

namespace TUF.GameMode
{
    [System.Serializable]
    public class GameModeComponentTimer : GameModeComponent
    {
        public enum TimerType
        {
            Stopwatch = 0,
            Countdown = 1
        }

        public TimerType Timer { get { return timerType; } }
        public int CurrentTime { get { return currentTime; } }

        /// <summary>
        /// The type of timer this is.
        /// </summary>
        [SerializeField] protected TimerType timerType = TimerType.Stopwatch;
        /// <summary>
        /// Current time in seconds.
        /// </summary>
        [SerializeField] [ReadOnly] protected int currentTime = 0;

        public override void InitComponent(GameModeBase GameMode)
        {
            base.InitComponent(GameMode);

        }
    }
}