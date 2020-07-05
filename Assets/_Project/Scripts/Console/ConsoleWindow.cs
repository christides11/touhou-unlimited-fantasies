using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TUF.Core
{
    public class ConsoleWindow : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI consoleText;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                container.SetActive(!container.activeSelf);
            }
        }

        public void Write(string text)
        {
            consoleText.text += text;
        }

        public void WriteLine(string text)
        {
            Write(text);
            Write("\n");
        }
    }
}