using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.GameMode
{
    [CreateAssetMenu(fileName = "GameModeDefiniton", menuName = "TAPI/GameModeDefinition")]
    public class GameModeDefinition : ScriptableObject
    {
        public string gameModeName;
        [TextArea] public string gameModeDescription;
        public GameModeHandler gameModeHandler;

        public bool characterSelect;
    }
}