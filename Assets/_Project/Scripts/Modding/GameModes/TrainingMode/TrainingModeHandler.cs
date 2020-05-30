﻿using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.GameMode
{
    public class TrainingModeHandler : GameModeHandler
    {
        public EntityController dummy;

        public bool frameByFrame = false;

        public override void StartGameMode(EntityDefinition character, StageDefinition scene,
            StageCollection stageCollection = null)
        {
            base.StartGameMode(character, scene, stageCollection);

            playerCharacters.Add(simObjectManager.SpawnObject(character.entity, scene.spawnPosition[0], Quaternion.identity));
            playerCamera.UpdateTarget(playerCharacters[0].transform);
            playerCharacters[0].GetComponent<EntityController>().Init(gameManager, playerCamera);
            playerCamera.Initialize(playerCharacters[0].GetComponent<EntityController>());

            //GameObject d = simObjectManager.SpawnObject(dummy.gameObject, scene.spawnPosition[0] + new Vector3(0, 0, 5), 
            //    Quaternion.identity);
            //d.GetComponent<EntityController>().Init(gameManager, null);
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                gamemodeActive = !gamemodeActive;
            }

            if (gamemodeActive == false)
            {
                if (Input.GetKeyDown(KeyCode.F3) || Input.GetKey(KeyCode.F4))
                {
                    Tick(1.0f/60.0f);
                }
            }
        }
    }
}