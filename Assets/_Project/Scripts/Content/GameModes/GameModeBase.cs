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
using System;
using TUF.Stages;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

        protected SpawnPointManager spawnPointManager;

        protected List<GameObject> playerCharacters = new List<GameObject>();
        protected PlayerCamera playerCamera;

        private StageCollection stageCollection;
        private StageDefinition currentStage;

        [EditorButton("Add Component")]
        public void TestMethod()
        {
#if UNITY_EDITOR
            GenericMenu menu = new GenericMenu();
            BuildItems();
            foreach (string t in gameModeComponentTypes.Keys)
            {
                string destination = t.Replace('.', '/');
                menu.AddItem(new GUIContent(destination), true, OnComponentSelected, t);
            }
            menu.AddSeparator("");
            menu.ShowAsContext();
#endif
        }

#if UNITY_EDITOR
        private Dictionary<string, Type> gameModeComponentTypes = new Dictionary<string, Type>();
        private void BuildItems()
        {
            gameModeComponentTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var givenType in a.GetTypes())
                {
                    if (givenType.IsSubclassOf(typeof(GameModeComponent)))
                    {
                        gameModeComponentTypes.Add(givenType.FullName, givenType);
                    }
                }
            }
        }

        protected void OnComponentSelected(object t)
        {
            Undo.RecordObject(this, "Added component.");
            components.Add((GameModeComponent)Activator.CreateInstance(gameModeComponentTypes[(string)t]));
        }
#endif

        [SerializeReference] protected List<GameModeComponent> components = new List<GameModeComponent>();


        /// <summary>
        /// Initializes the gamemode with any components it needs.
        /// Should be called before anything else.
        /// </summary>
        /// <param name="gameManager">the current Game Manager.</param>
        public virtual void InitGameMode(GameManager gameManager)
        {
            this.gameManager = gameManager;
            simObjectManager = new SimObjectManager();
            spawnPointManager = GameObject.FindObjectOfType<SpawnPointManager>();
            if(spawnPointManager == null)
            {
                string tx = $"<color=red>SCENE HAS NO SPAWNPOINTMANAGER!</color>";
                gameManager.ConsoleWindow.WriteLine(tx);
            }
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
            playerCamera = Instantiate(gameManager.gameVariables.playerCameraPrefab.gameObject, 
                spawnPointManager.GetSpawnPoint().position, Quaternion.identity)
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
