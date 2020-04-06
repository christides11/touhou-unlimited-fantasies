using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMod;
using System.IO;
using System;
using TAPI.Core;
using TAPI.GameMode;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TAPI.Entities.Shared;

namespace TAPI.Modding
{
    public class ModManager : MonoBehaviour
    {
        public static ModManager instance;

        public Dictionary<string, ModDefinition> mods = new Dictionary<string, ModDefinition>();

        public ModLoader ModLoader { get { return modLoader; } }

        [SerializeField] private GameManager gameManager;
        [SerializeField] private ModLoader modLoader;

        public void Init()
        {
            modLoader.Init(this, gameManager);
        }

        public GameModeDefinition GetGamemodeDefinition(ModGamemodeReference gamemode)
        {
            if (!mods.ContainsKey(gamemode.modIdentifier))
            {
                return null;
            }

            GameModeDefinition g = mods[gamemode.modIdentifier].GetGamemodeDefinition(gamemode.gamemodeName);

            if(g == null)
            {
                return null;
            }

            return g;
        }

        public List<ModGamemodeReference> GetGamemodeDefinitions()
        {
            List<ModGamemodeReference> gamemodes = new List<ModGamemodeReference>();

            foreach (string m in mods.Keys)
            {
                List<GameModeDefinition> gmds = mods[m].GetGamemodeDefinitions();
                foreach (GameModeDefinition gmd in gmds)
                {
                    gamemodes.Add(new ModGamemodeReference(m, gmd.gameModeName));
                }
            }

            return gamemodes;
        }

        public EntityDefinition GetEntity(ModEntityReference entity)
        {
            entity.modIdentifier = entity.modIdentifier.ToLower();
            if (!mods.ContainsKey(entity.modIdentifier))
            {
                return null;
            }

            EntityDefinition e = mods[entity.modIdentifier].GetEntityDefinition(entity.entityName);

            if(e == null)
            {
                return null;
            }

            return e;
        }

        public List<ModEntityReference> GetEntities()
        {
            List<ModEntityReference> entities = new List<ModEntityReference>();

            foreach (string m in mods.Keys)
            {
                List<EntityDefinition> eds = mods[m].GetEntityDefinitions();
                foreach (EntityDefinition ed in eds)
                {
                    entities.Add(new ModEntityReference(m, ed.entityName));
                }
            }

            return entities;
        }

        public StageDefinition GetStageDefinition(ModStageReference stage)
        {
            stage.modIdentifier = stage.modIdentifier.ToLower();
            if (!mods.ContainsKey(stage.modIdentifier))
            {
                return null;
            }

            StageDefinition s = mods[stage.modIdentifier].GetStageDefinition(stage.stageName);

            if(s == null)
            {
                return null;
            }

            return s;
        }

        /// <summary>
        /// Get a list of all stages available.
        /// </summary>
        /// <returns>A list that can be used to get the definition of any stage.</returns>
        public List<ModStageReference> GetStageDefinitions()
        {
            List<ModStageReference> stages = new List<ModStageReference>();

            foreach (string mod in mods.Keys)
            {
                List<StageDefinition> stageDefinitions = mods[mod].GetStageDefinitions();
                foreach (StageDefinition stageDefinition in stageDefinitions)
                {
                    stages.Add(new ModStageReference(mod, stageDefinition.stageName));
                }
            }

            return stages;
        }

        public StageCollection GetStageCollection(ModStageCollectionReference stageCollection)
        {
            stageCollection.modIdentifier = stageCollection.modIdentifier.ToLower();
            if (!mods.ContainsKey(stageCollection.modIdentifier))
            {
                return null;
            }

            StageCollection c = mods[stageCollection.modIdentifier].GetStageCollection(stageCollection.stageCollectionName);

            if(c == null)
            {
                return null;
            }

            return c;
        }

        public List<ModStageCollectionReference> GetStageCollections()
        {
            List<ModStageCollectionReference> stageCollections = new List<ModStageCollectionReference>();

            foreach(string m in mods.Keys)
            {
                List<StageCollection> scs = mods[m].GetStageCollections();
                foreach(StageCollection sc in scs)
                {
                    stageCollections.Add(new ModStageCollectionReference(m, sc.collectionName));
                }
            }

            return stageCollections;
        }

        public async Task<bool> LoadStage(ModStageReference stage)
        {
            if (mods.TryGetValue(stage.modIdentifier, out ModDefinition mod))
            {
                StageDefinition sd = mod.GetStageDefinition(stage.stageName);
                if (sd)
                {
                    if (mod.local)
                    {
                        await SceneManager.LoadSceneAsync(sd.sceneName, LoadSceneMode.Additive);
                        return true;
                    }
                    else
                    {
                        if (modLoader.loadedMods.TryGetValue(stage.modIdentifier, out ModHost modHost))
                        {
                            if (modHost.Scenes.Exists(sd.sceneName))
                            {
                                await modHost.Scenes.LoadAsync(sd.sceneName, true);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}