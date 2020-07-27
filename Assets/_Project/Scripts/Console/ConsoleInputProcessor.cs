using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUF.Core;
using TUF.Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TUF.Console
{
    public class ConsoleInputProcessor : MonoBehaviour
    {
        public GameManager gameManager;
        public ConsoleWindow consoleWindow;

        protected List<ConsoleCommand> preCommands = new List<ConsoleCommand>();
        protected List<ConsoleCommand> regularCommands = new List<ConsoleCommand>();
        protected List<ConsoleCommand> postCommands = new List<ConsoleCommand>();

        public async virtual Task Process(List<ConsoleInput> inputs)
        {
            for(int i = 0; i < inputs.Count; i++)
            {
                if(inputs[i].command == "+unloadallmods")
                {
                    preCommands.Add(new CommandUnloadAllMods().Initialize(this));
                }
                if(inputs[i].command == "+sgm")
                {
                    string[] s = inputs[i].variables[0].Split('/');
                    ModObjectReference entity = new ModObjectReference();
                    entity.modIdentifier = s[0];
                    entity.objectName = s[1];

                    s = inputs[i].variables[1].Split('/');
                    ModObjectReference gamemode = new ModObjectReference();
                    gamemode.modIdentifier = s[0];
                    gamemode.objectName = s[1];

                    s = inputs[i].variables[2].Split('/');
                    ModObjectReference stage = new ModObjectReference();
                    stage.modIdentifier = s[0];
                    stage.objectName = s[1];

                    CommandStartGameMode sgmCommand = new CommandStartGameMode(entity, gamemode, stage);

                    regularCommands.Add(sgmCommand.Initialize(this));
                }
            }

            foreach(ConsoleCommand cc in preCommands)
            {
                consoleWindow.WriteLine(await cc.Do());

            }
            foreach (ConsoleCommand cc in regularCommands)
            {
                consoleWindow.WriteLine(await cc.Do());
            }
            foreach (ConsoleCommand cc in postCommands)
            {
                consoleWindow.WriteLine(await cc.Do());
            }
        }
    }
}