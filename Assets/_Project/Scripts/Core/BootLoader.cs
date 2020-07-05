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

        public string editorArgs;

        private void Awake()
        {
            if(targetFramerate > 0)
            {
                Application.targetFrameRate = targetFramerate;
            }
        }

        async void Start()
        {
            modManager.Init();
            modManager.ModLoader.LoadAllMods();

            if (SceneManager.GetActiveScene().name != defaultScene)
            {
                await gameManager.LoadSceneAsync(defaultScene);
            }
        }
    }
}