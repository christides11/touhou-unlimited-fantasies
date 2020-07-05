using TUF.Console;
using TUF.Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            modManager.ModLoader.LoadAllMods();

            if (readEditorParams)
            {
                await consoleReader.Convert(editorArgs);
            }

            if (SceneManager.sceneCount == 1)
            {
                await gameManager.LoadSceneAsync(defaultScene);
            }
        }
    }
}