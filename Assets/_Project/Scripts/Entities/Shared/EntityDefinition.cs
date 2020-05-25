using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using UnityEngine;

namespace TUF.Entities.Shared
{
    [CreateAssetMenu(fileName = "EntityDefinition", menuName = "Entities/Definition")]
    public class EntityDefinition : ScriptableObject
    {
        public string entityName;
        public bool playerSelectable = true;
        public GameObject entity;
        public EntityStats stats;
        public MovesetDefinition moveset;
    }
}