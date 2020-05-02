using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "EntityChangeStateEvent", menuName = "Attack Events/ChangeState")]
    public class EntityChangeStateEvent : AttackEvent
    {
        public override void Evaluate(uint frame, uint endFrame,
            EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(AttackEventDefinition eventDefinition)
        {
            if(eventDefinition.variables.intVars == null
                || eventDefinition.variables.intVars.Count != 1)
            {
                eventDefinition.variables.intVars = new List<int>();
                eventDefinition.variables.intVars.Add(0);
            }

            eventDefinition.variables.intVars[0] = EditorGUILayout.IntField("State", 
                eventDefinition.variables.intVars[0]);
        }
#endif
    }
}
