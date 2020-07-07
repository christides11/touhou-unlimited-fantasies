using System;
using System.Collections;
using System.Collections.Generic;
using TUF.Entities.Shared;
using TUF.Modding;
using UnityEngine;

namespace TUF.Core
{
    public class EntitySpawner : MonoBehaviour
    {
        [System.Serializable]
        public class EntitySpawnInfo {
            public ModObjectLink entity;
            public Transform spawnPosition;
        }

        [SerializeField] private bool hasSpawned;
        [SerializeField] private TriggerDetector triggerDetector;
        [SerializeField] private List<EntitySpawnInfo> entitySpawns = new List<EntitySpawnInfo>();

        private void Awake()
        {
            if (triggerDetector)
            {
                triggerDetector.TriggerEnter += SpawnEntities;
            }
        }

        private void SpawnEntities(Collider other)
        {
            if (hasSpawned)
            {
                return;
            }

            GameManager gameManager = GameManager.current;
            for(int i = 0; i < entitySpawns.Count; i++)
            {
                EntityDefinition entityDefinition = gameManager.ModManager.GetEntity(entitySpawns[i].entity.reference);
                if(entityDefinition == null)
                {
                    continue;
                }

                gameManager.GameModeHanlder.SimObjectManager.SpawnObject(entityDefinition.entity, entitySpawns[i].spawnPosition.position,
                    Quaternion.identity);
            }
        }
    }
}