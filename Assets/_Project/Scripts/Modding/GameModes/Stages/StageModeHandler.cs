using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.GameMode
{
    public class StageModeHandler : GameModeHandler
    {

        public override void StartGameMode(EntityDefinition character, 
            StageDefinition currentStage, 
            StageCollection stageCollection = null)
        {
            base.StartGameMode(character, currentStage, stageCollection);

            playerCharacters.Add(simObjectManager.SpawnObject(character.entity, currentStage.spawnPosition[0], Quaternion.identity));
            playerCamera.UpdateTarget(playerCharacters[0].transform);
            playerCharacters[0].GetComponent<EntityController>().Init(gameManager, playerCamera, CAF.Input.InputControlType.Direct);
        }
    }
}