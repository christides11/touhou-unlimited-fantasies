﻿using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities;
using TUF.Entities.Shared;
using UnityEngine;

namespace TUF.GameMode
{
    public class GameModeTraining : GameModeBase
    {
        public EntityManager dummy;
        public bool frameByFrame = false;

        public override void StartGameMode(EntityDefinition character, StageDefinition scene,
            StageCollection stageCollection = null)
        {
            base.StartGameMode(character, scene, stageCollection);

            playerCharacters.Add(simObjectManager.SpawnObject(character.entity, spawnPointManager.GetSpawnPoint().position, Quaternion.identity));
            playerCamera.SetLookAtTarget(playerCharacters[0].transform);
            playerCharacters[0].GetComponent<EntityManager>().Init(gameManager, playerCamera, CAF.Input.InputControlType.Direct);
            playerCamera.Initialize(playerCharacters[0].GetComponent<EntityManager>());

            if (dummy != null)
            {
                GameObject d = simObjectManager.SpawnObject(dummy.gameObject, spawnPointManager.GetSpawnPoint().position + new Vector3(0, 0, 5),
                    Quaternion.identity);
                d.GetComponent<EntityManager>().Init(gameManager, null, CAF.Input.InputControlType.None);
            }
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