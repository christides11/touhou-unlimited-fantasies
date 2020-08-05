using UnityEngine;
using Kilosoft.Tools;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace TUF.GameMode
{
    public class GameModeComponentHolder : MonoBehaviour
    {
        [SerializeReference] protected GameModeComponentBase gameModeComponent;

        [EditorButton("Set Component")]
        public void SetComponent()
        {
#if UNITY_EDITOR
            GenericMenu menu = new GenericMenu();
            BuildItems();
            foreach (string t in gameModeComponentTypes.Keys)
            {
                string destination = t.Replace('.', '/');
                menu.AddItem(new GUIContent(destination), true, OnComponentSelected, t);
            }
            menu.ShowAsContext();
#endif
        }

#if UNITY_EDITOR
        private Dictionary<string, Type> gameModeComponentTypes = new Dictionary<string, Type>();
        private void BuildItems()
        {
            gameModeComponentTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(GameModeComponentBase)))
                    {
                        gameModeComponentTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        protected void OnComponentSelected(object t)
        {
            Undo.RecordObject(this, "Set component.");
            gameModeComponent = (GameModeComponentBase)Activator.CreateInstance(gameModeComponentTypes[(string)t]);
        }
#endif

        public GameModeComponentBase GetGameModeComponent()
        {
            return gameModeComponent;
        }
    }
}