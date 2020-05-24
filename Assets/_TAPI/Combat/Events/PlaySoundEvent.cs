using UnityEngine;
using System.Collections.Generic;
using TAPI.Sound;
using TAPI.Modding;
using CAF.Combat;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TAPI.Combat.Events
{
    public class PlaySoundEvent : AttackEvent
    {
        public override bool Evaluate(uint frame, uint endFrame, 
            CAF.Entities.EntityController controller, AttackEventVariables variables)
        {
            TAPI.Entities.EntityController c = (TAPI.Entities.EntityController)controller;
            SoundDefinition sound = c.GameManager.ModManager.GetSoundDefinition(((ModObjectLink)variables.objectVars[0]).reference);
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