using TAPI.Core;
using TAPI.Entities.Shared;
using TAPI.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
using GameManager = Touhou.Core.GameManager;

namespace Touhou.Menus.Options
{
    public class OptionsMenu : MonoBehaviour
    {
        public delegate void MenuExitedAction();
        public event MenuExitedAction OnMenuExited;

        private void Update()
        {
            if (GlobalInputManager.instance.GetButtonDown(0, TAPI.Inputs.Action.Cancel))
            {
                gameObject.SetActive(false);
                OnMenuExited?.Invoke();
            }
        }

        public void OpenControlSettings()
        {
            (GameManager.current as GameManager).cMapper.Open();
        }
    }
}