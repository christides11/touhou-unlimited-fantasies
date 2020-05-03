using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.GameMode
{
    public class TrainingModeHandler : GameModeHandler
    {
        public EntityController dummy;

        public override void StartGameMode(EntityDefinition character, StageDefinition scene,
            StageCollection stageCollection = null)
        {
            base.StartGameMode(character, scene, stageCollection);

            playerCharacters.Add(simObjectManager.SpawnObject(character.entity, scene.spawnPosition[0], Quaternion.identity));
            playerCamera.UpdateTarget(playerCharacters[0].transform);
            playerCharacters[0].GetComponent<EntityController>().Init(gameManager, playerCamera.Cam.transform);

            GameObject d = simObjectManager.SpawnObject(dummy.gameObject, scene.spawnPosition[0] + new Vector3(0, 0, 5), 
                Quaternion.identity);
            d.GetComponent<EntityController>().Init(gameManager, null);
        }
    }
}