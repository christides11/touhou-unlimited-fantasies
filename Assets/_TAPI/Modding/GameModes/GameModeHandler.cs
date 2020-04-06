﻿using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using TAPI.Entities.Shared;
using UnityEngine;

namespace TAPI.GameMode{
    public class GameModeHandler : MonoBehaviour
    {
        protected GameManager gameManager;
        protected TimeStepManager timeStepManager;
        protected SimObjectManager simObjectManager;
        protected bool started;

        protected GameObject player;
        protected PlayerCamera playerCamera;

        private StageCollection stageCollection;
        private StageDefinition currentStage;


        /// <summary>
        /// Initializes the gamemode. Call this before anything.
        /// </summary>
        /// <param name="gameManager">The gameManager in use.</param>
        public virtual void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
            simObjectManager = new SimObjectManager();
            timeStepManager = new TimeStepManager(60.0f, 1.0f, 120.0f, 30.0f);
            timeStepManager.OnUpdate += Tick;
        }

        /// <summary>
        /// Starts the gamemode.
        /// 
        /// </summary>
        /// <param name="character">The character the player selected.</param>
        /// <param name="currentStage">The current stage we are on.</param>
        /// <param name="stageCollection"></param>
        public virtual void StartGameMode(EntityDefinition character, StageDefinition currentStage,
            StageCollection stageCollection = null)
        {
            this.stageCollection = stageCollection;
            this.currentStage = currentStage;
            playerCamera = Instantiate(gameManager.playerCamera.gameObject, currentStage.spawnPosition, Quaternion.identity)
                .GetComponent<PlayerCamera>();
            timeStepManager.Activate();
            started = true;
        }

        public virtual void FinishGamemode()
        {

        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
            if (started)
            {
                timeStepManager.ManualUpdate(Time.fixedDeltaTime);
            }
        }

        public virtual void Tick(float dt)
        {
            TickUpdate(dt);
            TickLateUpdate(dt);
        }

        protected virtual void TickUpdate(float dt)
        {
            simObjectManager.Update(dt);
        }

        protected virtual void TickLateUpdate(float dt)
        {
            simObjectManager.LateUpdate(dt);
        }
    }
}