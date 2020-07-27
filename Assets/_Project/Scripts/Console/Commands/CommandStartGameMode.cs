using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUF.Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TUF.Console
{
    public class CommandStartGameMode : ConsoleCommand
    {
        ModObjectReference entity = new ModObjectReference();
        ModObjectReference gamemode = new ModObjectReference();
        ModObjectReference stage = new ModObjectReference();

        public CommandStartGameMode(ModObjectReference entity, ModObjectReference gameMode, ModObjectReference stage)
        {
            this.entity = entity;
            this.gamemode = gameMode;
            this.stage = stage;
        }

        public async override Task<string> Do()
        {
            string sceneToUnload = null;
            if (SceneManager.GetActiveScene().name != "Singletons")
            {
                sceneToUnload = SceneManager.GetActiveScene().name;
            }

            await processor.gameManager.StartGameMode(entity, gamemode, stage, null, sceneToUnload);

            return $"Game Mode <color=#05ed33>\"{gamemode.ToString()}\"</color> " +
                $"on stage <color=#05ed33>\"{stage.ToString()}\"</color> " +
                $"with character <color=#05ed33>\"{entity.ToString()}\"</color> has started.";
        }
    }
}