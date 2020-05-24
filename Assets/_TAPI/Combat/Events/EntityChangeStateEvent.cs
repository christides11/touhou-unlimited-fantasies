﻿using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat.Events
{
    public class EntityChangeStateEvent : AttackEvent
    {
        public override bool Evaluate(uint frame, uint endFrame,
            EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
            return false;
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
