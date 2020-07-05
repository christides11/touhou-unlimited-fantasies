using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUF.Console
{
    [System.Serializable]
    public class ConsoleInput
    {
        public string command;
        public List<string> variables = new List<string>();
    }
}