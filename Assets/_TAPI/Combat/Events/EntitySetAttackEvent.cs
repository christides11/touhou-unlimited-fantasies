using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat.Events
{
    public class EntitySetAttackEvent : AttackEvent
    {
        public override bool Evaluate(uint frame, uint endFrame,
    EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
            controller.CombatManager.Reset();
            controller.CombatManager.currentAttack = (MovesetAttackNode)variables.objectVars[0];
            controller.StateManager.ChangeState((int)EntityStates.ATTACK);
            return true;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(AttackEventDefinition eventDefinition)
        {
            if (eventDefinition.variables.objectVars == null
                || eventDefinition.variables.objectVars.Count != 1)
            {
                eventDefinition.variables.objectVars = new List<Object>();
                eventDefinition.variables.objectVars.Add(null);
            }

            eventDefinition.variables.objectVars[0] = EditorGUILayout.ObjectField("Attack", 
                eventDefinition.variables.objectVars[0], typeof(MovesetAttackNode), false);
        }
#endif
    }
}