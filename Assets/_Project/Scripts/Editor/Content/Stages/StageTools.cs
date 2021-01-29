using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace TUF.Stages
{
    public class StageTools
    {
        [MenuItem("TUF/Stage/Setup")]
        public static void SetupStage()
        {
            CreateSpawnManager();
        }

        private static void CreateSpawnManager()
        {
            if (GameObject.FindObjectOfType<SpawnPointManager>())
            {
                return;
            }
            GameObject spawnPointManager = new GameObject("SpawnPointManager");
            spawnPointManager.AddComponent<SpawnPointManager>();
        }
    }
}