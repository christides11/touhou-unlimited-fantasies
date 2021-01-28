using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Core
{
    [CreateAssetMenu(fileName = "GameVars", menuName = "TUF/Game Variables")]
    public class GameVariables : ScriptableObject
    {
        public PlayerCamera playerCameraPrefab;
        public int inputBuffer = 3;
        public float pushboxForce;
        public CombatVariables combat = new CombatVariables();
    }
}