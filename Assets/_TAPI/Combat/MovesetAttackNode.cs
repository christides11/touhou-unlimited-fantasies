using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TAPI.Combat
{
    [System.Serializable]
    public class MovesetAttackNode : Node
    {
        [Input] public MovesetAttackNode lastNode;

        public List<AttackButtonDefinition> executeButton = new List<AttackButtonDefinition>();
        public List<AttackButtonDefinition> buttonSequence = new List<AttackButtonDefinition>();
        public AttackSO action;
        public List<Vector2Int> cancelWindows = new List<Vector2Int>();

        [Output(dynamicPortList = true)] public List<MovesetAttackNode> nextNode = new List<MovesetAttackNode>();

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "nextNode")
            {
                return this;
            }
            else return null;
        }
    }
}