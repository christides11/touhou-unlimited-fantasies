using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using TUF.Core;
using TUF.Modding;

namespace TUF.Menus.MainMenu
{
    public class StageCollectionsMenu : MonoBehaviour
    {
        [SerializeField] private CharacterSelectMenu characterSelectMenu;
        [SerializeField] private StageCollectionItem stageCollectionItem;
        [SerializeField] private GameObject contentHolder;
        [SerializeField] private GameObject collectionStageItem;
        [SerializeField] private Transform collectionStageHolder;

        [Header("Collection Info")]
        [SerializeField] private GameObject collectionInfoParent;
        [SerializeField] private TextMeshProUGUI collectionInfoText;
        [SerializeField] private Button collectionStartButton;

        private void OnDisable()
        {
            foreach(Transform child in contentHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnEnable()
        {
            GameManager gm = GameManager.current;

            List<ModObjectReference> stageCollections = gm.ModManager.GetStageCollections();
            foreach (ModObjectReference sc in stageCollections)
            {
                StageCollection collection = gm.ModManager.GetStageCollection(sc);
                StageCollectionItem item = Instantiate(stageCollectionItem.gameObject, contentHolder.transform, false)
                    .GetComponent<StageCollectionItem>();
                item.collectionName.text = collection.collectionName;
                item.GetComponent<EventTrigger>().AddOnSubmitListeners((data) => { OnCollectionSelected(collection); });
            }
        }

        public void OnCollectionSelected(StageCollection sc)
        {
            foreach(Transform t in collectionStageHolder)
            {
                Destroy(t.gameObject);
            }

            int i = 1;
            foreach(GamemodeStageDefinition stage in sc.stages)
            {
                CollectionStageItem item = Instantiate(collectionStageItem.gameObject, collectionStageHolder.transform, false)
                    .GetComponent<CollectionStageItem>();
                item.stageText.text = $"Stage {i}";
                item.GetComponent<EventTrigger>().AddOnSubmitListeners((d) => { OnStageSelected(stage); });
                i++;
            }

            collectionInfoParent.SetActive(true);
            collectionInfoText.text = sc.collectionName;
            collectionStartButton.GetComponent<EventTrigger>().RemoveAllListeners();
        }

        public void OnStageSelected(GamemodeStageDefinition stage)
        {
            StageDefinition stageDefinition = GameManager.current.ModManager.GetStageDefinition(stage.stage);

            if(stageDefinition == null)
            {
                GameManager.current.ConsoleWindow.WriteLine($"Stage ({stage.ToString()}) not found.");
                return;
            }

            collectionInfoParent.SetActive(true);
            collectionInfoText.text = stageDefinition.stageName;
            collectionStartButton.GetComponent<EventTrigger>().RemoveAllListeners();
            collectionStartButton.GetComponent<EventTrigger>()
                .AddOnSubmitListeners((d) => { StageStartButton(stage.gamemode, stage.stage); });
        }

        private void StageStartButton(ModObjectReference gamemode, ModObjectReference stage)
        {
            characterSelectMenu.OnCharacterSelected += (d) => { StartStage(d, gamemode, stage); };
            characterSelectMenu.OnExit += () => { gameObject.SetActive(true); };
            characterSelectMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private void StartStage(ModObjectReference entity, ModObjectReference gamemode, ModObjectReference stage)
        {
            GameManager gm = GameManager.current;
            gm.StartGameMode(entity, gamemode, stage);
        }

        private void StartStageCollection()
        {

        }
    }
}