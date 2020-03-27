using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Entities.Shared;
using UnityEngine.EventSystems;
using Touhou.Core;
using TAPI.Core;
using GameManager = Touhou.Core.GameManager;
using TAPI.Inputs;
using GlobalInputManager = Touhou.Core.GlobalInputManager;
using TAPI.Modding;

namespace Touhou.Menus
{
    public class CharacterSelectMenu : MonoBehaviour
    {
        public delegate void ExitAction();
        public event ExitAction OnExit;
        public delegate void CharacterSelectedAction(EntityDefinition characterDefinition);
        public event CharacterSelectedAction OnCharacterSelected;

        private Dictionary<string, CharacterSelectContentItem> characters
            = new Dictionary<string, CharacterSelectContentItem>();

        [SerializeField] private Transform charactersTransform;
        [SerializeField] private CharacterSelectContentItem characterContentItem;

        private void OnEnable()
        {
            List<ModEntityReference> entityReferences = GameManager.current.ModManager.GetEntities();

            foreach(ModEntityReference entityReference in entityReferences)
            {
                EntityDefinition entityDefinition = GameManager.current.ModManager.GetEntity(entityReference);

                if (!entityDefinition.playerSelectable)
                {
                    continue;
                }

                GameObject go = Instantiate(characterContentItem.gameObject, charactersTransform, false);
                characters.Add(entityDefinition.entityName, go.GetComponent<CharacterSelectContentItem>());
                go.GetComponent<EventTrigger>().AddOnSubmitListeners((data) => { SelectedCharacter(entityDefinition); });
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

        private void SelectedCharacter(EntityDefinition ed)
        {
            Debug.Log($"Selected {ed.entityName}.");
            OnCharacterSelected?.Invoke(ed);
        }
    }
}