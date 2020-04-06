using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Core;
using TAPI.Modding;
using TAPI.GameMode;
using UnityEngine.EventSystems;
using TAPI.Entities.Shared;
using System;

namespace Touhou.Menus.MainMenu
{
    public class GamemodeSelectMenu : MonoBehaviour
    {
        public GamemodeContentItem contentItemPrefab;
        public Transform contentHolder;

        [Header("Menus")]
        public CharacterSelectMenu characterSelectMenu;
        public StageSelectMenu stageSelectMenu;

        private void OnEnable()
        {
            foreach(Transform t in contentHolder)
            {
                Destroy(t.gameObject);
            }

            GameManager gm = GameManager.current;

            List<ModGamemodeReference> gamemodeReferences = gm.ModManager.GetGamemodeDefinitions();

            foreach(ModGamemodeReference gamemodeReference in gamemodeReferences)
            {
                GameModeDefinition gamemodeDefinition = gm.ModManager.GetGamemodeDefinition(gamemodeReference);

                if (!gamemodeDefinition.selectable)
                {
                    continue;
                }

                GamemodeContentItem item = Instantiate(contentItemPrefab.gameObject, contentHolder, false)
                    .GetComponent<GamemodeContentItem>();
                item.gamemodeName.text = gamemodeDefinition.gameModeName;
                item.GetComponent<EventTrigger>().AddOnSubmitListeners((data) => { GamemodeSelected(gamemodeReference); });
            }
        }

        public void GamemodeSelected(ModGamemodeReference gamemode)
        {
            characterSelectMenu.gameObject.SetActive(true);
            characterSelectMenu.OnCharacterSelected += (entity) => { OpenStageSelect(gamemode, entity); };
            characterSelectMenu.OnExit += () => { gameObject.SetActive(true); };
            gameObject.SetActive(false);
        }

        private void OpenStageSelect(ModGamemodeReference gamemode, ModEntityReference entityReference)
        {
            stageSelectMenu.gameObject.SetActive(true);
            stageSelectMenu.OnStageSelected += (s) => { StartGamemode(gamemode, entityReference, s); };
            characterSelectMenu.gameObject.SetActive(false);
        }

        private void StartGamemode(ModGamemodeReference gamemode, ModEntityReference entity, ModStageReference stage)
        {
            GameManager.current.StartGameMode(entity, gamemode, stage);
        }
    }
}