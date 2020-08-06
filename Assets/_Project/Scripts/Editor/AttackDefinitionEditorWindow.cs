using System.Collections;
using System.Collections.Generic;
using TUF.Modding;
using UnityEditor;
using UnityEngine;

namespace TUF.Combat
{
    public class AttackDefinitionEditorWindow : CAF.Combat.AttackDefinitionEditorWindow
    {
        public static void Init(AttackDefinition attack)
        {
            AttackDefinitionEditorWindow window =
                (AttackDefinitionEditorWindow)EditorWindow.GetWindow(typeof(AttackDefinitionEditorWindow));
            window.attack = attack;
            window.Show();
        }

        protected override void DrawBoxGroupHitOptions(CAF.Combat.BoxGroup currentGroup)
        {
            base.DrawBoxGroupHitOptions(currentGroup);

            BoxGroup boxGroup = (BoxGroup)currentGroup;

            EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
            boxGroup.hitSound = (ModObjectLink)EditorGUILayout.ObjectField("Hit Sound", boxGroup.hitSound, typeof(ModObjectLink), false);
            EditorGUILayout.Space();
        }
    }
}