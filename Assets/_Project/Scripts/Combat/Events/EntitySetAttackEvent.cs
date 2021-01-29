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
        public bool resetFrameCounter = true;

        public override string GetName()
        {
            return "Set Attack";
        }

        public override bool Evaluate(uint frame, uint endFrame, CAF.Entities.EntityManager controller, AttackEventVariables variables)
        {
            controller.CombatManager.SetAttack((MovesetAttackNode)variables.objectVars[0]);
            controller.StateManager.ChangeState((int)EntityStates.ATTACK, resetFrameCounter ? 0 : controller.StateManager.CurrentStateFrame);
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

            resetFrameCounter = EditorGUILayout.Toggle("Reset Frame Counter", resetFrameCounter);

            eventDefinition.variables.objectVars[0] = EditorGUILayout.ObjectField("Attack", 
                eventDefinition.variables.objectVars[0], typeof(MovesetAttackNode), false);
        }
#endif
    }
}