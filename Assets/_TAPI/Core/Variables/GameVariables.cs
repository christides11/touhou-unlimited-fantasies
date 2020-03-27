using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Core
{
    [CreateAssetMenu(fileName = "GameVars", menuName = "TAPI/Core/Game Variables")]
    public class GameVariables : ScriptableObject
    {
        public int inputBuffer = 3;
        public float pushboxForce;
        public CombatVariables combat = new CombatVariables();
    }
}