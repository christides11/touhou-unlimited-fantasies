﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace TAPI.Combat
{
	[CreateAssetMenu(fileName = "MovesetDefinition", menuName = "Combat/Moveset")]
	public class MovesetDefinition : NodeGraph
	{
		[Header("Attacks")]
		public List<MovesetAttackNode> groundAttackCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> groundAttackStartNodes = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> airAttackCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> airAttackStartNodes = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> floatingAttackCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> floatingAttackStartNodes = new List<MovesetAttackNode>();

		[Header("Bullet")]
		public List<MovesetAttackNode> groundBulletCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> groundBulletStartNodes = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> airBulletCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> airBulletStartNodes = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> floatingBulletCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> floatingBulletStartNodes = new List<MovesetAttackNode>();

		[Header("Special")]
		public List<MovesetAttackNode> groundSpecialCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> groundSpecialStartNodes = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> airSpecialCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> airSpecialStartNodes = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> floatingSpecialCommandNormals = new List<MovesetAttackNode>();
		public List<MovesetAttackNode> floatingSpecialStartNodes = new List<MovesetAttackNode>();
	}
}