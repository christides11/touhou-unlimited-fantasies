using System.Collections;
using System.Collections.Generic;
using TAPI.Combat;
using UnityEngine;

namespace TAPI.Entities.Shared
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