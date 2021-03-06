﻿using UnityEngine;
using System.Collections.Generic;
using TUF.Sound;
using TUF.Modding;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TUF.Combat.Events
{
    public class PlaySoundEvent : AttackEvent
    {
        public bool dmy;

        public override string GetName()
        {
            return "Play Sound";
        }

        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityManager controller, AttackEventVariables variables)
        {
            TUF.Entities.EntityManager c = (TUF.Entities.EntityManager)controller;
            SoundDefinition sound = c.GameManager.ModManager.GetSoundDefinition(((ModObjectLink)variables.objectVars[0]).reference);
            if (sound)
            {
                SoundManager.Play(sound, 0, controller.transform);
            }
            return false;
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

            eventDefinition.variables.objectVars[0] = EditorGUILayout.ObjectField("Sound", eventDefinition.variables.objectVars[0],
                typeof(ModObjectLink), false);
        }
#endif
    }
}