using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TAPI.Combat
{
	[CreateAssetMenu(fileName = "MovesetDefinition", menuName = "Combat/Moveset")]
	public class MovesetDefinition : NodeGraph
	{
		public List<MovesetAttackNode> groundCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> groundStartNodes = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> floatingStartNodes = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> airCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> airStartNodes = new List<MovesetAttackNode>();
	}
}