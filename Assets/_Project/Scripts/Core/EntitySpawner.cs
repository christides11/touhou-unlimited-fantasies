﻿using System;
using System.Collections;
using System.Collections.Generic;
using TUF.Combat;
using TUF.Entities;
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
            public EntityTeams team;
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

                GameObject entity = gameManager.GameModeHanlder.SimObjectManager.SpawnObject(entityDefinition.entity, entitySpawns[i].spawnPosition.position,
                    Quaternion.identity);

                entity.GetComponent<EntityCombatManager>().SetTeam(EntityTeams.Enemy);
                entity.GetComponent<CAF.Entities.HealthManager>().OnHurt += EntityHealthCheck;
                entity.GetComponent<CAF.Entities.HealthManager>().OnHealthSet += EntityHealthCheck;
            }

            hasSpawned = true;
        }

        private void EntityHealthCheck(GameObject gameObject, float oldHealth, float currentHealth)
        {
            GameManager gameManager = GameManager.current;

            if (currentHealth <= 0)
            {
                gameManager.GameModeHanlder.SimObjectManager.DestroyObject(gameObject.GetComponent<CAF.Simulation.SimObject>());
            }
        }
    }
}