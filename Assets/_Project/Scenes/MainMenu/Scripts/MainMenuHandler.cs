using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;
using GameManager = Touhou.Core.GameManager;

namespace Touhou.Menus.MainMenu
{
    public class MainMenuHandler : MonoBehaviour
    {
        [SerializeField] private CharacterSelectMenu characterSelect;
        [SerializeField] private StageCollectionsMenu stageCollectionsMenu;
        [SerializeField] private GamemodeSelectMenu gamemodeSelectMenu;
        [SerializeField] private string trainingModeScene;
        [SerializeField] private string TrainingModeIdentifier;

        public void ButtonStageCollections()
        {
            stageCollectionsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ButtonGameModes()
        {
            gamemodeSelectMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ButtonOptionsMenu()
        {

        }
        public void ButtonExit()
        {

        }

        private void OnReturnToMainMenu()
        {
            gameObject.SetActive(true);
        }
    }
}