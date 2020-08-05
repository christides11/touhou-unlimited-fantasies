using TUF.Core;
using TUF.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TUF.Menus.Options
{
    public class ModsMenu : MonoBehaviour
    {
        public delegate void MenuExitedAction();
        public event MenuExitedAction OnMenuExited;

        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject manageModsMenu;

        private void Update()
        {
            if (GlobalInputManager.instance.GetButtonDown(0, TUF.Inputs.Action.Cancel))
            {
                if (EventSystem.current.currentSelectedGameObject?.GetComponent<TMPro.TMP_InputField>())
                {
                    return;
                }
                if (GameManager.current.modBrowser.gameObject.activeInHierarchy)
                {
                    GameManager.current.modBrowser.gameObject.SetActive(false);
                    mainMenu.SetActive(true);
                    return;
                }
                if (manageModsMenu.activeInHierarchy)
                {
                    mainMenu.SetActive(true);
                    manageModsMenu.SetActive(false);
                    return;
                }
                gameObject.SetActive(false);
                OnMenuExited?.Invoke();
            }
        }

        public void ButtonModIOBrowser()
        {
            mainMenu.SetActive(false);
            GameManager.current.modBrowser.gameObject.SetActive(true);
        }

        public void ButtonUIManageMods()
        {
            mainMenu.SetActive(false);
            manageModsMenu.SetActive(true);
        }

        public void ButtonUIConfiguration()
        {

        }
    }
}