﻿using TUF.Core;
using TUF.Menus.Options;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TUF.Menus.MainMenu
{
    public class MainMenuHandler : MonoBehaviour
    {
        private GameObject lastSelection;

        [SerializeField] private CharacterSelectMenu characterSelect;
        [SerializeField] private StageCollectionsMenu stageCollectionsMenu;
        [SerializeField] private GamemodeSelectMenu gamemodeSelectMenu;
        [SerializeField] private OptionsMenu optionsMenu;
        [SerializeField] private ModsMenu modsMenu;
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
            optionsMenu.OnMenuExited += ReturnFromOptionsMenu;
            optionsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ButtonModMenu()
        {
            modsMenu.OnMenuExited += ReturnFromModsMenu;
            modsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private void ReturnFromOptionsMenu()
        {
            gameObject.SetActive(true);
            optionsMenu.OnMenuExited -= ReturnFromOptionsMenu;
        }

        private void ReturnFromModsMenu()
        {
            gameObject.SetActive(true);
            modsMenu.OnMenuExited -= ReturnFromModsMenu;
        }

        public void ButtonExit()
        {

        }
    }
}