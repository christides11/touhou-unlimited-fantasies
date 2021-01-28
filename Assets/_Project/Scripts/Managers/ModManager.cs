using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMod;
using System.IO;
using System;
using TUF.Core;
using TUF.GameMode;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TUF.Entities.Shared;
using TUF.Sound;

namespace TUF.Modding
{
    public class ModManager : MonoBehaviour
    {
        public static ModManager instance;

        public Dictionary<string, IModDefinition> mods = new Dictionary<string, IModDefinition>();

        public ModLoader ModLoader { get { return modLoader; } }

        [SerializeField] private GameManager gameManager;
        [SerializeField] private ModLoader modLoader;

        public void Init()
        {
            modLoader.Init(this, gameManager);
        }

        public async Task<GameModeDefinition> GetGamemodeDefinition(ModObjectReference gamemode)
        {
            if (!mods.ContainsKey(gamemode.modIdentifier))
            {
                return null;
            }

            GameModeDefinition g = await mods[gamemode.modIdentifier].GetGamemodeDefinition(gamemode.objectName);

            if(g == null)
            {
                return null;
            }

            return g;
        }

        public async Task<List<ModObjectReference>> GetGamemodeDefinitions()
        {
            List<ModObjectReference> gamemodes = new List<ModObjectReference>();

            foreach (string m in mods.Keys)
            {
                List<GameModeDefinition> gmds = await mods[m].GetGamemodeDefinitions();
                foreach (GameModeDefinition gmd in gmds)
                {
                    gamemodes.Add(new ModObjectReference(m, gmd.gameModeID));
                }
            }

            return gamemodes;
        }

        public async Task<EntityDefinition> GetEntity(ModObjectReference entity)
        {
            entity.modIdentifier = entity.modIdentifier.ToLower();
            if (!mods.ContainsKey(entity.modIdentifier))
            {
                return null;
            }

            EntityDefinition e = await mods[entity.modIdentifier].GetEntityDefinition(entity.objectName);

            if(e == null)
            {
                return null;
            }

            return e;
        }

        public async Task<List<ModObjectReference>> GetEntities()
        {
            List<ModObjectReference> entities = new List<ModObjectReference>();

            foreach (string m in mods.Keys)
            {
                List<EntityDefinition> eds = await mods[m].GetEntityDefinitions();
                foreach (EntityDefinition ed in eds)
                {
                    entities.Add(new ModObjectReference(m, ed.entityName));
                }
            }

            return entities;
        }

        public async Task<StageDefinition> GetStageDefinition(ModObjectReference stage)
        {
            stage.modIdentifier = stage.modIdentifier.ToLower();
            if (!mods.ContainsKey(stage.modIdentifier))
            {
                return null;
            }

            StageDefinition s = await mods[stage.modIdentifier].GetStageDefinition(stage.objectName);

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
        public async Task<List<ModObjectReference>> GetStageDefinitions()
        {
            List<ModObjectReference> stages = new List<ModObjectReference>();

            foreach (string mod in mods.Keys)
            {
                List<StageDefinition> stageDefinitions = await mods[mod].GetStageDefinitions();
                foreach (StageDefinition stageDefinition in stageDefinitions)
                {
                    stages.Add(new ModObjectReference(mod, stageDefinition.stageName));
                }
            }

            return stages;
        }

        public async Task<StageCollection> GetStageCollection(ModObjectReference stageCollection)
        {
            stageCollection.modIdentifier = stageCollection.modIdentifier.ToLower();
            if (!mods.ContainsKey(stageCollection.modIdentifier))
            {
                return null;
            }

            StageCollection c = await mods[stageCollection.modIdentifier].GetStageCollection(stageCollection.objectName);

            if(c == null)
            {
                return null;
            }

            return c;
        }

        public async Task<List<ModObjectReference>> GetStageCollections()
        {
            List<ModObjectReference> stageCollections = new List<ModObjectReference>();

            foreach(string m in mods.Keys)
            {
                List<StageCollection> scs = await mods[m].GetStageCollections();
                foreach(StageCollection sc in scs)
                {
                    stageCollections.Add(new ModObjectReference(m, sc.collectionID));
                }
            }

            return stageCollections;
        }

        public async Task<List<string>> LoadStage(ModObjectReference stage)
        {
            List<string> result = new List<string>(1);
            if (mods.TryGetValue(stage.modIdentifier, out IModDefinition mod))
            {
                StageDefinition sd = await mod.GetStageDefinition(stage.objectName);
                if (sd)
                {
                    if (mod.LocalMod)
                    {
                        result = await mod.LoadStageAsync(sd.stageIdentifier, LoadSceneMode.Additive);
                        return result;
                    }
                    else
                    {
                        if (modLoader.loadedMods.TryGetValue(stage.modIdentifier, out ModHost modHost))
                        {
                            for(int i = 0; i < sd.sceneNames.Length; i++)
                            {
                                if (!modHost.Scenes.Exists(sd.sceneNames[i]))
                                {
                                    result.Add(null);
                                    break;
                                }
                                await modHost.Scenes.LoadAsync(sd.sceneNames[i], true);
                                result.Add(sd.sceneNames[i]);
                            }
                        }
                    }
                }
            }
            return result;
        }

        #region Sounds
        public SoundDefinition GetSoundDefinition(ModObjectReference sound)
        {
            if (sound == null)
            {
                return null;
            }

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
        #endregion
    }
}