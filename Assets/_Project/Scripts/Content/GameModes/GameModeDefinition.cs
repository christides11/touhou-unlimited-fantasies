using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.GameMode
{
    [CreateAssetMenu(fileName = "GameModeDefiniton", menuName = "TUF/Content/GameModes/GameModeDefinition")]
    public class GameModeDefinition : ScriptableObject
    {
        public string gameModeID;
        public string gameModeName;
        [TextArea] public string gameModeDescription;
        public GameModeBase gameModeHandler;

        public bool selectable = true;
        public bool characterSelect;
    }
}