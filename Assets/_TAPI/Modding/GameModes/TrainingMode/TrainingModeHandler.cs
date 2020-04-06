﻿using System.Collections;
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

            player = simObjectManager.SpawnObject(character.entity, scene.spawnPosition, Quaternion.identity);
            playerCamera.UpdateTarget(player.transform);
            player.GetComponent<EntityController>().Init(gameManager, playerCamera.Cam.transform);

            GameObject d = simObjectManager.SpawnObject(dummy.gameObject, scene.spawnPosition + new Vector3(0, 0, 5), 
                Quaternion.identity);
            d.GetComponent<EntityController>().Init(gameManager, null);
        }
    }
}