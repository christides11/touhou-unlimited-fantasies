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
using TAPI.Sound;

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

        public GameModeDefinition GetGamemodeDefinition(ModObjectReference gamemode)
        {
            if (!mods.ContainsKey(gamemode.modIdentifier))
            {
                return null;
            }

            GameModeDefinition g = mods[gamemode.modIdentifier].GetGamemodeDefinition(gamemode.objectName);

            if(g == null)
            {
                return null;
            }

            return g;
        }

        public List<ModObjectReference> GetGamemodeDefinitions()
        {
            List<ModObjectReference> gamemodes = new List<ModObjectReference>();

            foreach (string m in mods.Keys)
            {
                List<GameModeDefinition> gmds = mods[m].GetGamemodeDefinitions();
                foreach (GameModeDefinition gmd in gmds)
                {
                    gamemodes.Add(new ModObjectReference(m, gmd.gameModeName));
                }
            }

            return gamemodes;
        }

        public EntityDefinition GetEntity(ModObjectReference entity)
        {
            entity.modIdentifier = entity.modIdentifier.ToLower();
            if (!mods.ContainsKey(entity.modIdentifier))
            {
                return null;
            }

            EntityDefinition e = mods[entity.modIdentifier].GetEntityDefinition(entity.objectName);

            if(e == null)
            {
                return null;
            }

            return e;
        }

        public List<ModObjectReference> GetEntities()
        {
            List<ModObjectReference> entities = new List<ModObjectReference>();

            foreach (string m in mods.Keys)
            {
                List<EntityDefinition> eds = mods[m].GetEntityDefinitions();
                foreach (EntityDefinition ed in eds)
                {
                    entities.Add(new ModObjectReference(m, ed.entityName));
                }
            }

            return entities;
        }

        public StageDefinition GetStageDefinition(ModObjectReference stage)
        {
            stage.modIdentifier = stage.modIdentifier.ToLower();
            if (!mods.ContainsKey(stage.modIdentifier))
            {
                return null;
            }

            StageDefinition s = mods[stage.modIdentifier].GetStageDefinition(stage.objectName);

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
        public List<ModObjectReference> GetStageDefinitions()
        {
            List<ModObjectReference> stages = new List<ModObjectReference>();

            foreach (string mod in mods.Keys)
            {
                List<StageDefinition> stageDefinitions = mods[mod].GetStageDefinitions();
                foreach (StageDefinition stageDefinition in stageDefinitions)
                {
                    stages.Add(new ModObjectReference(mod, stageDefinition.stageName));
                }
            }

            return stages;
        }

        public StageCollection GetStageCollection(ModObjectReference stageCollection)
        {
            stageCollection.modIdentifier = stageCollection.modIdentifier.ToLower();
            if (!mods.ContainsKey(stageCollection.modIdentifier))
            {
                return null;
            }

            StageCollection c = mods[stageCollection.modIdentifier].GetStageCollection(stageCollection.objectName);

            if(c == null)
            {
                return null;
            }

            return c;
        }

        public List<ModObjectReference> GetStageCollections()
        {
            List<ModObjectReference> stageCollections = new List<ModObjectReference>();

            foreach(string m in mods.Keys)
            {
                List<StageCollection> scs = mods[m].GetStageCollections();
                foreach(StageCollection sc in scs)
                {
                    stageCollections.Add(new ModObjectReference(m, sc.collectionName));
                }
            }

            return stageCollections;
        }

        public async Task<bool> LoadStage(ModObjectReference stage)
        {
            if (mods.TryGetValue(stage.modIdentifier, out ModDefinition mod))
            {
                StageDefinition sd = mod.GetStageDefinition(stage.objectName);
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

        public SoundDefinition GetSoundDefinition(ModObjectReference sound)
        {
            if (!mods.ContainsKey(sound.modIdentifier))
            {
                return null;
            }

            SoundDefinition g = mods[sound.modIdentifier].GetSoundDefinition(sound.objectName);

            if (g == null)
            {
                return null;
            }

            return g;
        }

        public List<ModObjectReference> GetSoundDefinitions()
        {
            List<ModObjectReference> sounds = new List<ModObjectReference>();

            foreach (string m in mods.Keys)
            {
                List<SoundDefinition> gmds = mods[m].GetSoundDefinitions();
                foreach (SoundDefinition gmd in gmds)
                {
                    sounds.Add(new ModObjectReference(m, gmd.soundName));
                }
            }

            return sounds;
        }
    }
}