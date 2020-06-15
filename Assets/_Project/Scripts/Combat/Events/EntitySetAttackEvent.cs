using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Events
{
    public class EntitySetAttackEvent : AttackEvent
    {
        public bool dmy;

        public override string GetName()
        {
            return "Set Attack";
        }

        public override bool Evaluate(uint frame, uint endFrame, CAF.Entities.EntityController controller, AttackEventVariables variables)
        {
            controller.CombatManager.Cleanup();
            controller.CombatManager.CurrentAttack = (MovesetAttackNode)variables.objectVars[0];
            controller.StateManager.ChangeState((int)EntityStates.ATTACK);
            return true;
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
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