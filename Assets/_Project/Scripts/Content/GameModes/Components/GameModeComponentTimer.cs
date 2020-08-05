using UnityEngine;
using ReadOnlyAttribute = TUF.Core.ReadOnlyAttribute;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.GameMode
{
    [System.Serializable]
    public class GameModeComponentTimer : GameModeComponentBase
    {
        [System.Serializable]
        public class TimerData : GameModeComponentData
        {
            /// <summary>
            /// The type of timer this is.
            /// </summary>
            [SerializeField] public TimerType timerType = TimerType.Stopwatch;
            /// <summary>
            /// Current time.
            /// </summary>
            [SerializeField] [ReadOnly] public float currentTime = 0;

            public override void DrawInspectorUI()
            {
#if UNITY_EDITOR
                timerType = (TimerType)EditorGUILayout.EnumPopup("Timer Type", timerType);
                currentTime = EditorGUILayout.FloatField(timerType == TimerType.Stopwatch ? "Start Time" : "Total Time", currentTime);
#endif
            }
        }

        public enum TimerType
        {
            Stopwatch = 0,
            Countdown = 1
        }

        public TimerType Timer { get { return timerData.timerType; } }
        public float CurrentTime { get { return timerData.currentTime; } }
        [SerializeField] [HideInInspector] protected TimerData timerData = new TimerData();

        public override void InitComponent(GameModeBase GameMode)
        {
            base.InitComponent(GameMode);

        }

        public override GameModeComponentData GetComponentData()
        {
            return timerData;
        }

        public override void SetComponentData(GameModeComponentData componentData)
        {
            timerData = (TimerData)componentData;
        }
    }
}