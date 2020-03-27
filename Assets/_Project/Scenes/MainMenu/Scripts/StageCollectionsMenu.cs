using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAPI.Core;
using TAPI.Modding;
using UnityEngine.EventSystems;

namespace Touhou.Menus.MainMenu
{
    public class StageCollectionsMenu : MonoBehaviour
    {
        [SerializeField] private StageCollectionItem stageCollectionItem;
        [SerializeField] private GameObject contentHolder;
        [SerializeField] private GameObject collectionStageItem;
        [SerializeField] private Transform collectionStageHolder;

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

            List<ModStageCollectionReference> stageCollections = gm.ModManager.GetStageCollections();
            foreach (ModStageCollectionReference sc in stageCollections)
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
            foreach(ModStageReference stage in sc.stages)
            {
                CollectionStageItem item = Instantiate(collectionStageItem.gameObject, collectionStageHolder.transform, false)
                    .GetComponent<CollectionStageItem>();
                item.stageText.text = $"Stage {i}";
                i++;
            }
        }
    }
}