using KinematicCharacterController;
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

        public virtual void Init(GameManager gameManager)
        {
            this.gameManager = gameManager;
            simObjectManager = new SimObjectManager();
            timeStepManager = new TimeStepManager(60.0f, 1.0f, 120.0f, 30.0f);
            timeStepManager.OnUpdate += Tick;
        }

        public virtual void StartGameMode(StageDefinition scene, EntityDefinition character)
        {
            playerCamera = Instantiate(gameManager.playerCamera.gameObject, scene.spawnPosition, Quaternion.identity)
                .GetComponent<PlayerCamera>();
            timeStepManager.Activate();
            started = true;
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

        public virtual void TickUpdate(float dt)
        {
            simObjectManager.Update(dt);
        }

        public virtual void TickLateUpdate(float dt)
        {
            simObjectManager.LateUpdate(dt);
        }
    }
}
