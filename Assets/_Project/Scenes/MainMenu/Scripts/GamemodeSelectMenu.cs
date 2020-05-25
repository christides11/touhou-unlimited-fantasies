using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;
using TUF.Modding;
using TUF.Core;
using TUF.GameMode;
using TUF.Inputs;

namespace TUF.Menus.MainMenu
{
    public class GamemodeSelectMenu : MonoBehaviour
    {
        public GamemodeContentItem contentItemPrefab;
        public Transform contentHolder;

        [Header("Menus")]
        public GameObject mainMenu;
        public CharacterSelectMenu characterSelectMenu;
        public StageSelectMenu stageSelectMenu;

        [Header("Gamemode")]
        public TextMeshProUGUI gamemodeName;
        public Image gamemodeImage;
        public TextMeshProUGUI gamemodeDescription;

        private void OnEnable()
        {
            Cleanup();

            GameManager gm = GameManager.current;

            List<ModObjectReference> gamemodeReferences = gm.ModManager.GetGamemodeDefinitions();

            foreach(ModObjectReference gamemodeReference in gamemodeReferences)
            {
                GameModeDefinition gamemodeDefinition = gm.ModManager.GetGamemodeDefinition(gamemodeReference);

                if (!gamemodeDefinition.selectable)
                {
                    continue;
                }

                GamemodeContentItem item = Instantiate(contentItemPrefab.gameObject, contentHolder, false)
                    .GetComponent<GamemodeContentItem>();
                item.gamemodeName.text = gamemodeDefinition.gameModeName;
                item.GetComponent<EventTrigger>().AddOnSelectedListeners((data) => { GamemodeSelected(gamemodeReference); });
                item.GetComponent<EventTrigger>().AddOnSubmitListeners((data) => { GamemodeSubmitted(gamemodeReference); });
            }

            gamemodeName.text = "Gamemode Select";
            gamemodeDescription.text = "Select a gamemode.";
        }

        private void Update()
        {
            if(GlobalInputManager.instance.GetButtonDown(0, TUF.Inputs.Action.Cancel))
            {
                OnBack();
            }
        }

        public void OnBack()
        {
            Cleanup();
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        private void Cleanup()
        {
            foreach (Transform t in contentHolder)
            {
                Destroy(t.gameObject);
            }
        }

        public void GamemodeSelected(ModObjectReference gamemode)
        {
            GameModeDefinition gm = GameManager.current.ModManager.GetGamemodeDefinition(gamemode);
            if (gm)
            {
                gamemodeName.text = gm.gameModeName;
                gamemodeDescription.text = gm.gameModeDescription;
            }
        }

        public void GamemodeSubmitted(ModObjectReference gamemode)
        {
            characterSelectMenu.gameObject.SetActive(true);
            characterSelectMenu.OnCharacterSelected += (entity) => { OpenStageSelect(gamemode, entity); };
            characterSelectMenu.OnExit += () => { gameObject.SetActive(true); };
            gameObject.SetActive(false);
        }

        private void OpenStageSelect(ModObjectReference gamemode, ModObjectReference entityReference)
        {
            stageSelectMenu.gameObject.SetActive(true);
            stageSelectMenu.OnStageSelected += (s) => { StartGamemode(gamemode, entityReference, s); };
            characterSelectMenu.gameObject.SetActive(false);
        }

        private void StartGamemode(ModObjectReference gamemode, ModObjectReference entity, ModObjectReference stage)
        {
            GameManager.current.StartGameMode(entity, gamemode, stage);
        }
    }
}