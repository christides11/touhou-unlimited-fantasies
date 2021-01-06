using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.GameMode
{
    /// <summary>
    /// The "Stage" Game Mode is about getting to the finish point, usually
    /// with enemies and bosses in between that. When you reach the finish, 
    /// you're given a score based on a number of factors that can be customized.
    /// </summary>
    public class GameModeStage : GameModeBase
    {

        public override void StartGameMode(EntityDefinition character, 
            StageDefinition currentStage, 
            StageCollection stageCollection = null)
        {
            base.StartGameMode(character, currentStage, stageCollection);

            playerCharacters.Add(simObjectManager.SpawnObject(character.entity, spawnPointManager.GetSpawnPoint().position, Quaternion.identity));
            playerCamera.SetLookAtTarget(playerCharacters[0].transform);
            playerCharacters[0].GetComponent<EntityManager>().Init(gameManager, playerCamera, CAF.Input.InputControlType.Direct);
        }
    }
}