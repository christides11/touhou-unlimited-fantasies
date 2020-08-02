using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee.List;

namespace TUF.Stages
{
    public class SpawnPointManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

        public int SpawnPointCount()
        {
            return spawnPoints.Count;
        }

        public Transform GetSpawnPoint()
        {
            return GetSpawnPoint(0);
        }

        public Transform GetSpawnPoint(int index)
        {
            return spawnPoints[0];
        }
    }
}