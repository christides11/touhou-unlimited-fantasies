using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TUF.Console;
using System;

namespace TUF.Core
{
    public class ConsoleWindow : MonoBehaviour
    {
        public Camera consoleWindowCamera;
        [SerializeField] private ConsoleReader consoleReader;
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI consoleText;
        [SerializeField] private TMP_InputField inputField;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                container.SetActive(!container.activeSelf);
            }
            if(Input.GetKeyDown(KeyCode.Return) && !String.IsNullOrEmpty(inputField.text))
            {
                string input = inputField.text;
                inputField.text = "";
                _ = consoleReader.Convert(input.Split(' '));
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