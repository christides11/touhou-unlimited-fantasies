using TUF.Core;
using TUF.Inputs;
using UnityEngine;

namespace TUF.Menus.Options
{
    public class OptionsMenu : MonoBehaviour
    {
        public delegate void MenuExitedAction();
        public event MenuExitedAction OnMenuExited;

        private void Update()
        {
            if (GlobalInputManager.instance.GetButtonDown(0, TUF.Inputs.Action.Cancel))
            {
                gameObject.SetActive(false);
                OnMenuExited?.Invoke();
            }
        }

        public void OpenControlSettings()
        {
            GameManager.current.controlMapper.Open();
        }
    }
}