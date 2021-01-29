using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TUF.Console;
using System;

namespace TUF.Core
{
    public enum ConsoleMessageType
    {
        Debug = 0,
        Error = 1,
        Warning = 2,
        Print = 3
    }
    public class ConsoleWindow : MonoBehaviour
    {
        public Camera consoleWindowCamera;
        [SerializeField] private ConsoleReader consoleReader;
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI consoleText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private List<Color> messageColors = new List<Color>(4);

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
                WriteLine($"> {input}", ConsoleMessageType.Print);
                _ = consoleReader.Convert(input.Split(' '));
            }
        }

        public void Write(string text)
        {
            consoleText.text += text;
        }

        public void WriteLine(string text, ConsoleMessageType msgType = ConsoleMessageType.Debug)
        {
            Write($"<#{ColorUtility.ToHtmlStringRGBA(messageColors[(int)msgType])}>" + text + "</color>");
            Write("\n");
        }
    }
}