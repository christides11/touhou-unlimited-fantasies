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

        protected GameModeHandler currentGameModeHandler;
        private EntityDefinition selectedCharacter;

        [SerializeField] private List<ModDefinition> internalMods = new List<ModDefinition>();

        [SerializeField] private ModManager modManager;

        [Header("Prefabs")]
        public GameVariables gameVars;
        public PlayerCamera playerCamera;

        protected virtual void Awake()
        {
            current = this;
            for(int i = 0; i < internalMods.Count; i++)
            {
                internalMods[i].local = true;
                modManager.mods.Add($"christides11.tidespack", internalMods[i]);
            }
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
        /// <param name="character"></param>
        /// <param name="gameMode"></param>
        /// <param name="scene"></param>
        public virtual async void StartGameMode(EntityDefinition character, ModGamemodeReference gameMode, ModStageReference stage) { 
            LoadGameMode(modManager.GetGamemodeDefinition(gameMode));
            StageDefinition wantedScene = modManager.GetStageDefinition(stage);

            bool result = await modManager.LoadStage(stage);
            if (!result)
            {
                Debug.Log($"Error loading stage {stage.modIdentifier}/{stage.stageName}.");
                return;
            }
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(wantedScene.sceneName));

            currentGameModeHandler.Init(this);
            currentGameModeHandler.StartGameMode(wantedScene, character);
        }

        /// <summary>
        /// Loads the given gamemode and sets it as the active one.
        /// </summary>
        /// <param name="gameModeDefinition"></param>
        protected virtual void LoadGameMode(GameModeDefinition gameModeDefinition)
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