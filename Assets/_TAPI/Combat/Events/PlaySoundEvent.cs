﻿using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;
using System.Collections.Generic;
using TAPI.Sound;
using TAPI.Modding;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat.Events
{
    [CreateAssetMenu(fileName = "PlaySoundEvent", menuName = "Attack Events/Play Sound")]
    public class PlaySoundEvent : AttackEvent
    {
        public override bool Evaluate(uint frame, uint endFrame,
            EntityAttack attackState, EntityController controller, AttackEventVariables variables)
        {
            SoundDefinition sound = controller.GameManager.ModManager.GetSoundDefinition(((ModObjectLink)variables.objectVars[0]).reference);
            if (sound)
            {
                SoundManager.Play(sound, 0, controller.transform);
            }
            return false;
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

            eventDefinition.variables.objectVars[0] = EditorGUILayout.ObjectField("Sound", eventDefinition.variables.objectVars[0],
                typeof(ModObjectLink), false);
        }
#endif
    }
}