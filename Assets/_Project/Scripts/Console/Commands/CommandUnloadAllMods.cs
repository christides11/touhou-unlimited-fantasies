using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUF.Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TUF.Console
{
    public class CommandUnloadAllMods : ConsoleCommand
    {
        public async override Task<string> Do()
        {
            processor.gameManager.ModManager.ModLoader.UnloadAllMods();

            return "Unloaded all mods!";
        }
    }
}