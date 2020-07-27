using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TUF.Console
{
    public class ConsoleReader : MonoBehaviour
    {
        [SerializeField] private ConsoleInputProcessor inputProcessor;

        public async Task ReadCommandLine()
        {
            await Convert(System.Environment.GetCommandLineArgs());
        }

        public async Task Convert(string[] input)
        {
            List<ConsoleInput> inputs = new List<ConsoleInput>();

            for(int i = 0; i < input.Length; i++)
            {
                string[] s = input[i].Split('=');
                ConsoleInput ci = new ConsoleInput();
                ci.command = s[0];
                if (s.Length > 1) {
                    string[] variables = s[1].Split(',');
                    for(int j = 0; j < variables.Length; j++)
                    {
                        ci.variables.Add(variables[j]);
                    }
                }
                inputs.Add(ci);
            }

            await inputProcessor.Process(inputs);
        }
    }
}