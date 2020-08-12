using UnityEngine;

namespace TUF.GameMode
{
    [System.Serializable]
    public class GameModeComponentStyleMeter : GameModeComponentBase
    {
        [System.Serializable]
        public class StyleMeterData : GameModeComponentData
        {
            [SerializeField] public float styleDepletionTime = 1.5f;
        }

        [SerializeField] [HideInInspector] protected StyleMeterData styleMeterData = new StyleMeterData();

        public override void InitComponent(GameModeBase GameMode)
        {
            base.InitComponent(GameMode);
        }

        public override GameModeComponentData GetComponentData()
        {
            return styleMeterData;
        }
    }
}