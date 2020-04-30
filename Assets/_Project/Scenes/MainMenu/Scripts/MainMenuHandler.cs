using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;
using UnityEngine.EventSystems;
using GameManager = Touhou.Core.GameManager;

namespace Touhou.Menus.MainMenu
{
    public class MainMenuHandler : MonoBehaviour
    {
        private GameObject lastSelection;

        [SerializeField] private CharacterSelectMenu characterSelect;
        [SerializeField] private StageCollectionsMenu stageCollectionsMenu;
        [SerializeField] private GamemodeSelectMenu gamemodeSelectMenu;
        [SerializeField] private string trainingModeScene;
        [SerializeField] private string TrainingModeIdentifier;

        [SerializeField] private GameObject defaultSelection;

        private void OnEnable()
        {
            if (lastSelection == null)
            {
                lastSelection = defaultSelection;
            }

            EventSystem.current.SetSelectedGameObject(lastSelection);
        }

        private void Update()
        {
            Helpers.SelectDefaultSelection(defaultSelection);
        }

        public void ButtonStageCollections()
        {
            lastSelection = EventSystem.current.currentSelectedGameObject;
            stageCollectionsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ButtonGameModes()
        {
            lastSelection = EventSystem.current.currentSelectedGameObject;
            gamemodeSelectMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ButtonOptionsMenu()
        {

        }
        public void ButtonExit()
        {

        }
    }
}