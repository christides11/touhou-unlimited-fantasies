using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TUF.Modding;
using TUF.Entities.Shared;
using TUF.GameMode;
using Rewired.UI.ControlMapper;
using TUF.Inputs;

namespace TUF.Core
{
    /// <summary>
    /// The Game Manager keeps references to important scripts 
    /// and provides functionally for handling the overall game state.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// A singleton referring to the currently active Game Manager.
        /// </summary>
        public static GameManager current;

        public ModManager ModManager { get { return modManager; } }
        public ConsoleWindow ConsoleWindow { get { return consoleWindow; } }
        /// <summary>
        /// Returns the current Game Mode that we are in.
        /// If we aren't in any Game Mode, returns null.
        /// </summary>
        public GameModeBase GameMode { get; protected set; } = null;

        /// <summary>
        /// A definition of content that's included with TUF.
        /// </summary>
        [SerializeField] private LocalModDefinition coreMod;
        /// <summary>
        /// A definition of a mod included with TUF.
        /// </summary>
        [SerializeField] private LocalModDefinition internalMod;

        [Header("References")]
        [SerializeField] private ModManager modManager;
        [SerializeField] private ConsoleWindow consoleWindow;
        [SerializeField] private MusicManager musicManager;
        [SerializeField] public ModIO.UI.ModBrowser modBrowser;
        /// <summary>
        /// The UI that allows us to change our controls. Comes from Rewired.
        /// </summary>
        public ControlMapper controlMapper;

        /// <summary>
        /// Stores variables and prefabs that are commonly used.
        /// </summary>
        [Header("Prefabs")]
        public GameVariables gameVariables;

        /// <summary>
        /// Initializes the Game Manager. This is called by the Boot Loader,
        /// so don't call this manually.
        /// </summary>
        public virtual void Initialize()
        {
            GlobalInputManager.instance = new GlobalInputManager();
            current = this;
            modManager.mods.Add("core", coreMod);
            modManager.mods.Add("christides11.tidespack", internalMod);
        }

        /// <summary>
        /// Loads the given scene, and returns the name of the
        /// current active scene.
        /// </summary>
        /// <param name="SceneName">The name of the scene to load.</param>
        /// <param name="setActiveScene">If the scene should be set as the active one.</param>
        /// <returns>The scene that was active before this operation.</returns>
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
        /// <param name="entity">The Entity the player will be controlling.</param>
        /// <param name="gamemode">The Game Mode the player will be playing.</param>
        /// <param name="stage">The Stage the player will be playing on.</param>
        /// <param name="stageCollection">The Stage Collection that will be played through, if applicable.</param>
        /// <param name="sceneToUnload">The scene to unload. This should be any scene that's loaded besides Singletons.</param>
        /// <returns></returns>
        public virtual async Task StartGameMode(ModObjectReference entity, ModObjectReference gamemode, ModObjectReference stage,
            ModObjectReference stageCollection = null, string sceneToUnload = null) {
            EntityDefinition entityDefinition = await modManager.GetEntity(entity);
            GameModeDefinition gamemodeDefinition = await modManager.GetGamemodeDefinition(gamemode);
            StageDefinition stageDefinition = await modManager.GetStageDefinition(stage);
            StageCollection stageCollectionDefinition = stageCollection != null ? await modManager.GetStageCollection(stageCollection)
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

            List<string> scenesLoaded = await modManager.LoadStage(stage);
            if (scenesLoaded.Contains(null))
            {
                Debug.Log($"Error loading stage {stage.ToString()}.");
                return;
            }
            // Unloads scenes that aren't the singletons scene.
            if (sceneToUnload != null)
            {
                await SceneManager.UnloadSceneAsync(sceneToUnload);
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenesLoaded[0]));

            GameMode.InitGameMode(this);
            GameMode.StartGameMode(entityDefinition, stageDefinition);
        }

        /// <summary>
        /// Loads the given Game Mode and sets it as the active one.
        /// </summary>
        /// <param name="gameModeDefinition">The Game Mode to load.</param>
        protected virtual void SetGameMode(GameModeDefinition gameModeDefinition)
        {
            if (this.GameMode)
            {
                Destroy(this.GameMode.gameObject);
            }
            GameObject gameMode = Instantiate(gameModeDefinition.gameModeHandler.gameObject, transform);
            this.GameMode = gameMode.GetComponent<GameModeBase>();
        }
    }
}