using System.Collections;
using System.Collections.Generic;
using TAPI.Core;
using Touhou.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Touhou.Core
{
    public class BootLoader : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TAPI.Modding.ModManager modManager;

        [SerializeField] private string defaultScene;

        [SerializeField] private int targetFramerate = -1;

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