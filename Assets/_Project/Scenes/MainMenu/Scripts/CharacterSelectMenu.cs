using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameManager = TUF.Core.GameManager;
using TUF.Modding;
using TUF.Entities.Shared;
using TUF.Core;
using TUF.Inputs;

namespace TUF.Menus
{
    public class CharacterSelectMenu : MonoBehaviour
    {
        public delegate void ExitAction();
        public event ExitAction OnExit;
        public delegate void CharacterSelectedAction(ModObjectReference entity);
        public event CharacterSelectedAction OnCharacterSelected;

        private Dictionary<string, CharacterSelectContentItem> characters
            = new Dictionary<string, CharacterSelectContentItem>();

        [SerializeField] private Transform charactersTransform;
        [SerializeField] private CharacterSelectContentItem characterContentItem;

        private void OnEnable()
        {
            List<ModObjectReference> entityReferences = GameManager.current.ModManager.GetEntities();

            foreach(ModObjectReference entityReference in entityReferences)
            {
                EntityDefinition entityDefinition = GameManager.current.ModManager.GetEntity(entityReference);

                if (!entityDefinition.playerSelectable)
                {
                    continue;
                }

                GameObject go = Instantiate(characterContentItem.gameObject, charactersTransform, false);
                characters.Add(entityDefinition.entityName, go.GetComponent<CharacterSelectContentItem>());
                go.GetComponent<EventTrigger>().AddOnSubmitListeners((data) => { SelectedCharacter(entityReference); });
            }
        }

        private void OnDisable()
        {
            foreach(CharacterSelectContentItem cs in characters.Values)
            {
                Destroy(cs.gameObject);
            }
            characters.Clear();
        }

        private void Update()
        {
            if(GlobalInputManager.instance.GetButtonDown(0, Action.Cancel))
            {
                OnExit?.Invoke();
                gameObject.SetActive(false);
            }
        }

        private void SelectedCharacter(ModObjectReference entityReference)
        {
            GameManager.current.ConsoleWindow.WriteLine($"Selected {entityReference.ToString()}.");
            OnCharacterSelected?.Invoke(entityReference);
        }
    }
}