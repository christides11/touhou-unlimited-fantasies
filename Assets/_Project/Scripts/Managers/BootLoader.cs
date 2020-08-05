using TUF.Console;
using TUF.Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModIO;
using System.Collections.Generic;
using ModManager = TUF.Modding.ModManager;

namespace TUF.Core
{
    /// <summary>
    /// The Boot Loader is the first script to run in the game, and it
    /// handles initilization of the Game Manager and mods before loading the
    /// Main Menu, along with a few other task.
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        /// <summary>
        /// The Game Manager to initialize.
        /// </summary>
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ModManager modManager;

        /// <summary>
        /// What scene we should load into after initilization.
        /// </summary>
        [SerializeField] private string defaultScene;
        /// <summary>
        /// The target framerate of the game. -1 if unlimited.
        /// </summary>
        [SerializeField] private int targetFramerate = -1;

        [SerializeField]private ConsoleReader consoleReader;

        /// <summary>
        /// If editorArgs should be read by the Console Reader.
        /// Only applies in-editor.
        /// </summary>
        public bool readEditorParams;
        // The console arguments to use in-editor.
        public string[] editorArgs;

        private void Awake()
        {
            if(targetFramerate > 0)
            {
                Application.targetFrameRate = targetFramerate;
            }
        }

        async void Start()
        {
            gameManager.Initialize();
            modManager.Init();

            if (readEditorParams && Application.isEditor)
            {
                await consoleReader.Convert(editorArgs);
            }
            else
            {
                await consoleReader.ReadCommandLine();
            }

            if (SceneManager.sceneCount == 1)
            {
                await gameManager.LoadSceneAsync(defaultScene);
            }
        }
    }
}