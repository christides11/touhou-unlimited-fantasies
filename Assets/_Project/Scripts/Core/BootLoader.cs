using TUF.Console;
using TUF.Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModIO;
using System.Collections.Generic;
using ModManager = TUF.Modding.ModManager;

namespace TUF.Core
{
    public class BootLoader : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ModManager modManager;

        [SerializeField] private string defaultScene;

        [SerializeField] private int targetFramerate = -1;

        [SerializeField]private ConsoleReader consoleReader;

        public bool readEditorParams;
        public string[] editorArgs;

        public List<string> mods;

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
            mods = ModIO.ModManager.GetInstalledModDirectories(true);
            modManager.Init();
            modManager.ModLoader.LoadAllMods();

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