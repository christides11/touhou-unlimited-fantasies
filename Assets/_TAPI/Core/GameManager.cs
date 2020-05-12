using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TAPI.Modding;
using TAPI.Entities.Shared;
using TAPI.GameMode;

namespace TAPI.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager current;

        public ModManager ModManager { get { return modManager; } }
        public GameModeHandler GameModeHanlder { get { return currentGameModeHandler; } }
        public ConsoleWindow ConsoleWindow { get { return consoleWindow; } }

        protected GameModeHandler currentGameModeHandler;

        [SerializeField] private ModDefinition coreMod;
        [SerializeField] private ModDefinition internalMod;

        [Header("References")]
        [SerializeField] private ModManager modManager;
        [SerializeField] private ConsoleWindow consoleWindow;
        [SerializeField] private MusicManager musicManager;

        [Header("Prefabs")]
        public GameVariables gameVars;
        public PlayerCamera playerCamera;

        protected virtual void Awake()
        {
            current = this;
            coreMod.local = true;
            modManager.mods.Add("core", coreMod);
            modManager.mods.Add("christides11.tidespack", internalMod);
        }

        /// <summary>
        /// Loads the given scene, and returns the name of the
        /// current active scene.
        /// </summary>
        /// <param name="SceneName"></param>
        /// <returns></returns>
        public virtual async Task<string> LoadSceneAsync(string SceneName, bool setActiveScene = true)
        {
            string currentActiveScene = SceneManager.GetActiveScene().name;
            await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            if (setActiveScene)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
            }
            return currentActiveScene;
        }

        /// <summary>
        /// Loads the gamemode and the scene provided,
        /// then starts the gamemode.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="gamemode"></param>
        /// <param name="stage"></param>
        public virtual async void StartGameMode(ModObjectReference entity, ModObjectReference gamemode, ModObjectReference stage,
            ModObjectReference stageCollection = null) {
            EntityDefinition entityDefinition = modManager.GetEntity(entity);
            GameModeDefinition gamemodeDefinition = modManager.GetGamemodeDefinition(gamemode);
            StageDefinition stageDefinition = modManager.GetStageDefinition(stage);
            StageCollection stageCollectionDefinition = stageCollection != null ? modManager.GetStageCollection(stageCollection)
                : null;

            // Error checking.
            if(entityDefinition == null)
            {
                Debug.Log($"Can not find entity {entity.ToString()}.");
                return;
            }
            if(gamemodeDefinition == null)
            {
                Debug.Log($"Can not find gamemode {gamemode.ToString()}.");
                return;
            }
            if(stageDefinition == null)
            {
                Debug.Log($"Can not find stage {stage.ToString()}.");
                return;
            }

            // Load everything and start gamemode.
            SetGameMode(gamemodeDefinition);

            bool result = await modManager.LoadStage(stage);
            if (!result)
            {
                Debug.Log($"Error loading stage {stage.ToString()}.");
                return;
            }
            // Unloads scenes that aren't the singletons scene.
            if (SceneManager.sceneCount > 1)
            {
                await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(stageDefinition.sceneName));

            currentGameModeHandler.Initialize(this);
            currentGameModeHandler.StartGameMode(entityDefinition, stageDefinition);
        }

        /// <summary>
        /// Loads the given gamemode and sets it as the active one.
        /// </summary>
        /// <param name="gameModeDefinition"></param>
        protected virtual void SetGameMode(GameModeDefinition gameModeDefinition)
        {
            if (currentGameModeHandler)
            {
                Destroy(currentGameModeHandler.gameObject);
            }
            GameObject gameMode = Instantiate(gameModeDefinition.gameModeHandler.gameObject, transform);
            currentGameModeHandler = gameMode.GetComponent<GameModeHandler>();
        }
    }
}