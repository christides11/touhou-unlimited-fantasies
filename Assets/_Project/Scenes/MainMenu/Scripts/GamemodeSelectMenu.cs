using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Core;
using TAPI.Modding;
using TAPI.GameMode;
using UnityEngine.EventSystems;

namespace Touhou.Menus.MainMenu
{
    public class GamemodeSelectMenu : MonoBehaviour
    {
        public GamemodeContentItem contentItemPrefab;
        public Transform contentHolder;

        [Header("Menus")]
        public CharacterSelectMenu characterSelectMenu;

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

                GamemodeContentItem item = Instantiate(contentItemPrefab.gameObject, contentHolder, false)
                    .GetComponent<GamemodeContentItem>();
                item.gamemodeName.text = gamemodeDefinition.gameModeName;
                item.GetComponent<EventTrigger>().AddOnSubmitListeners((data) => { GamemodeSelected(gamemodeDefinition); });
            }
        }

        public void GamemodeSelected(GameModeDefinition gamemode)
        {

        }
    }
}