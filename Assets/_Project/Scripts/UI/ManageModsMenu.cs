using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUF.Core;
using TUF.Modding;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUF.UI
{
    public class ManageModsMenu : MonoBehaviour
    {
        [System.Serializable]
        public enum ModFilterType
        {
            ALL = 0,
            ENABLED = 1,
            DISABLED = 2
        }

        private ModFilterType modFilter = ModFilterType.ALL;

        public GameObject modListItem;
        public Transform modListContnentHolder;
        public TMP_InputField filterField;

        [Header("Mod Info")]
        public TextMeshProUGUI modNameText;
        public Button modStatusButton;

        private void OnEnable()
        {
            filterField.onValueChanged.AddListener((data) => 
            {
                RefreshList(data);
            });

            RefreshList();
        }

        private void RefreshList()
        {
            RefreshList(filterField.text);
        }

        private void RefreshList(string filter = "")
        {
            foreach (Transform child in modListContnentHolder)
            {
                Destroy(child.gameObject);
            }

            ModLoader modLoader = GameManager.current.ModManager.ModLoader;

            for (int i = 0; i < modLoader.modList.Count; i++)
            {
                bool modLoaded = modLoader.loadedMods.ContainsKey(modLoader.modList[i].identifier);
                if (!string.IsNullOrEmpty(filter) && !modLoader.modList[i].modName.ToLower().Contains(filter.ToLower()))
                {
                    continue;
                }
                if(modFilter == ModFilterType.DISABLED && modLoaded)
                {
                    continue;
                }
                if (modFilter == ModFilterType.ENABLED && !modLoaded)
                {
                    continue;
                }
                string modIdentifier = modLoader.modList[i].identifier;
                GameObject go = GameObject.Instantiate(modListItem, modListContnentHolder, false);
                go.GetComponentInChildren<TextMeshProUGUI>().text = modLoader.modList[i].modName;
                int ind = i;
                go.GetComponent<EventTrigger>().RemoveAllListeners();
                go.GetComponent<EventTrigger>().AddOnSubmitListeners((data) =>
                {
                    OnModSelected(ind);
                });
            }
        }

        public void SetModFilter(int filterType)
        {
            modFilter = (ModFilterType)filterType;
            RefreshList();
        }

        private void OnModSelected(int index)
        {
            ModLoader modLoader = GameManager.current.ModManager.ModLoader;
            ModInfo mi = modLoader.modList[index];
            bool modLoaded = modLoader.loadedMods.ContainsKey(mi.identifier);

            modNameText.text = mi.modName;
            modStatusButton.GetComponentInChildren<TextMeshProUGUI>().text = modLoaded ? "Disable" : "Enable";
            int ind = index;
            modStatusButton.GetComponent<EventTrigger>().RemoveAllListeners();
            modStatusButton.GetComponent<EventTrigger>().AddOnSubmitListeners((data) =>
            {
                ButtonModStatusToggle(ind);
            });
        }

        private void ButtonModStatusToggle(int index)
        {
            ModLoader modLoader = GameManager.current.ModManager.ModLoader;
            ModInfo mi = modLoader.modList[index];
            bool modLoaded = modLoader.loadedMods.ContainsKey(mi.identifier);

            if (modLoaded)
            {
                modLoader.UnloadMod(mi.identifier);
            }
            else
            {
                modLoader.LoadMod(mi.identifier);
            }
            modLoaded = modLoader.loadedMods.ContainsKey(mi.identifier);

            modStatusButton.GetComponentInChildren<TextMeshProUGUI>().text = modLoaded ? "Disable" : "Enable";
        }
    }
}