using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TUF.Modding;
using TUF.Core;

namespace TUF.Menus
{
    public class StageSelectMenu : MonoBehaviour
    {
        public delegate void StageSelectedAction(ModObjectReference stage);
        public event StageSelectedAction OnStageSelected;

        public StageContentItem stageContentItemPrefab;
        public Transform stageContentHolder;

        private int currentPage = 0;
        [SerializeField] private int stagesPerPage = 30;

        private List<ModObjectReference> stageReferences;

        private void OnEnable()
        {
            stageReferences = GameManager.current.ModManager.GetStageDefinitions();

            currentPage = 0;
            DisplayPage();
        }

        private void DisplayPage()
        {
            while (currentPage != 0 && stageReferences.Count < (stagesPerPage * currentPage))
            {
                currentPage--;
            }
            foreach (Transform child in stageContentHolder)
            {
                Destroy(child.gameObject);
            }

            for(int i = stagesPerPage * currentPage; i < (stagesPerPage * currentPage) + stagesPerPage; i++)
            {
                if(i >= stageReferences.Count)
                {
                    return;
                }

                ModObjectReference stageReference = stageReferences[i];

                StageDefinition stageDefinition = GameManager.current.ModManager.GetStageDefinition(stageReferences[i]);
                if (!stageDefinition.selectableForGamemodes)
                {
                    continue;
                }

                StageContentItem stageItem = Instantiate(stageContentItemPrefab.gameObject, stageContentHolder, false)
                    .GetComponent<StageContentItem>();
                stageItem.eventTrigger.AddOnSubmitListeners((d) => { StageSelected(stageReference); });
            }
        }

        private void StageSelected(ModObjectReference stageReference)
        {
            OnStageSelected?.Invoke(stageReference);
            gameObject.SetActive(false);
        }
    }
}