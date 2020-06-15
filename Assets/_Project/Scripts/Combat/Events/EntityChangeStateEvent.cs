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
    public class EntityChangeStateEvent : AttackEvent
    {
        public override string GetName()
        {
            return "Change State";
        }

        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityController controller, AttackEventVariables variables)
        {
            return base.Evaluate(frame, endFrame, controller, variables);
        }

#if UNITY_EDITOR
        public override void DrawEventVariables(CAF.Combat.AttackEventDefinition eventDefinition)
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
