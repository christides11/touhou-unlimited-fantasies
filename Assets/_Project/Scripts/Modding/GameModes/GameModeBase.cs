using CAF.Simulation;
using Kilosoft.Tools;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using TUF.Core;
using TUF.Entities.Shared;
using UnityEngine;
using ReadOnlyAttribute = TUF.Core.ReadOnlyAttribute;
using SimObjectManager = TUF.Core.SimObjectManager;

namespace TUF.GameMode{
    /// <summary>
    /// Defines the game mode being played, it's rules, scoring, etc.
    /// </summary>
    public class GameModeBase : MonoBehaviour
    {
        public SimObjectManager SimObjectManager { get { return simObjectManager; } }
        public bool GamemodeActive { get { return gamemodeActive; } }

        public IReadOnlyCollection<GameModeComponent> GameModeComponents { get { return components.AsReadOnly(); } }

        protected GameManager gameManager;
        protected SimObjectManager simObjectManager;
        protected bool gamemodeActive;

        protected List<GameObject> playerCharacters = new List<GameObject>();
        protected PlayerCamera playerCamera;

        private StageCollection stageCollection;
        private StageDefinition currentStage;

        [SerializeReference] protected List<GameModeComponent> components = new List<GameModeComponent>();


        [EditorButton("Add Component")]
        public void TestMethod()
        {

        }

        /// <summary>
        /// Initializes the gamemode with any components it needs.
        /// Should be called before anything else.
        /// </summary>
        /// <param name="gameManager">the current Game Manager.</param>
        public virtual void InitGameMode(GameManager gameManager)
        {
            this.gameManager = gameManager;
            simObjectManager = new SimObjectManager();
        }

        /// <summary>
        /// Starts the gamemode.
        /// </summary>
        /// <param name="character">The character the player selected.</param>
        /// <param name="currentStage">The current stage we are on.</param>
        /// <param name="stageCollection"></param>
        public virtual void StartGameMode(EntityDefinition character, StageDefinition currentStage,
            StageCollection stageCollection = null)
        {
            this.stageCollection = stageCollection;
            this.currentStage = currentStage;
            playerCamera = Instantiate(gameManager.playerCamera.gameObject, currentStage.spawnPosition[0], Quaternion.identity)
                .GetComponent<PlayerCamera>();
            ActivateGamemode();
        }

        public virtual void DeactivateGamemode()
        {
            gamemodeActive = false;
        }

        public virtual void ActivateGamemode()
        {
            gamemodeActive = true;
        }

        public virtual void FinishGamemode()
        {

        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
            if (gamemodeActive)
            {
                Tick(Time.fixedDeltaTime);
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
