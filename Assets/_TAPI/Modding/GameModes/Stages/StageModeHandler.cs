using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.GameMode
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
            playerCharacters[0].GetComponent<EntityController>().Init(gameManager, playerCamera.Cam.transform);
        }
    }
}