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
        [SerializeField] private GameManager gameManager;

        public async Task Process(List<ConsoleInput> inputs)
        {
            for(int i = 0; i < inputs.Count; i++)
            {
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

                    string sceneToUnload = null;
                    if(SceneManager.GetActiveScene().name != "Singletons")
                    {
                        sceneToUnload = SceneManager.GetActiveScene().name;
                    }

                    await gameManager.StartGameMode(entity, gamemode, stage, null, sceneToUnload);
                }
            }
        }
    }
}