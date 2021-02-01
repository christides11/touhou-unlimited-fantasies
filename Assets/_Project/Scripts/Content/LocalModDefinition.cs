using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUF.Core;
using TUF.Entities.Shared;
using TUF.GameMode;
using TUF.Sound;
using TUF.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

namespace TUF.Modding
{
    [CreateAssetMenu(fileName = "LocalModDefinition", menuName = "TUF/Modding/LocalModDefinition")]
    public class LocalModDefinition : ScriptableObject, IModDefinition
    {
        public bool LocalMod { get { return true; } }
        public string Description { get { return description; } }

        [TextArea] [SerializeField] private string description;
        [SerializeField] private List<AssetReference> entityRefs = new List<AssetReference>();
        [SerializeField] private List<AssetReference> gameModeRefs = new List<AssetReference>();
        [SerializeField] private List<AssetReference> stageCollectionRefs = new List<AssetReference>();
        [SerializeField] private List<AssetReference> stageRefs = new List<AssetReference>();
        [SerializeField] private List<AssetReference> sounds = new List<AssetReference>();
        [SerializeField] private List<AssetReference> songs = new List<AssetReference>();
        [SerializeField] private List<AssetReference> uis = new List<AssetReference>();
        [SerializeField] private List<AssetReference> uiOverrides = new List<AssetReference>();

        private List<EntityDefinition> entities = new List<EntityDefinition>();
        private List<GameModeDefinition> gamemodes = new List<GameModeDefinition>();
        private List<StageDefinition> stages = new List<StageDefinition>();
        private List<StageCollection> stageCollections = new List<StageCollection>();

        private async Task<List<T>> LoadRealData<T>(List<AssetReference> refs, Action<T> callback)
        {
            var hh = await Addressables.LoadAssetsAsync<T>(refs, callback, Addressables.MergeMode.Union).Task;
            return hh.ToList();
        }

        #region Entities
        public async virtual Task<EntityDefinition> GetEntityDefinition(string entityName)
        {
            if(entities.Count == 0)
            {
                await GetEntityDefinitions();
            }
            return entities.FirstOrDefault(x => x.entityName.Equals(entityName));
        }

        public async virtual Task<List<EntityDefinition>> GetEntityDefinitions()
        {
            if(entities.Count == 0 && entityRefs.Count > 0)
            {
                entities = await LoadRealData<EntityDefinition>(entityRefs, null);
            }
            return entities;
        }
        #endregion

        #region GameModes
        public async Task<GameModeDefinition> GetGamemodeDefinition(string gamemodeID)
        {
            if(gamemodes.Count == 0)
            {
                await GetGamemodeDefinitions();
            }
            return gamemodes.FirstOrDefault(x => x.gameModeID == gamemodeID);
        }

        public async Task<List<GameModeDefinition>> GetGamemodeDefinitions()
        {
            if(gamemodes.Count == 0 && gameModeRefs.Count > 0)
            {
                gamemodes = await LoadRealData<GameModeDefinition>(gameModeRefs, null);
            }
            return gamemodes;
        }
        #endregion

        #region Songs
        public SoundDefinition GetSongDefinition(string songName)
        {
            throw new System.NotImplementedException();
        }

        public List<SoundDefinition> GetSongDefinitions()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Sounds
        public SoundDefinition GetSoundDefinition(string soundName)
        {
            throw new System.NotImplementedException();
        }

        public List<SoundDefinition> GetSoundDefinitions()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Stage Collections
        public async Task<StageCollection> GetStageCollection(string collectionID)
        {
            if (stageCollections.Count == 0)
            {
                await GetStageCollections();
            }
            return stageCollections.FirstOrDefault(x => x.collectionID == collectionID);
        }

        public async Task<List<StageCollection>> GetStageCollections()
        {
            if (stageCollections.Count == 0 && stageCollectionRefs.Count > 0)
            {
                stageCollections = await LoadRealData<StageCollection>(stageCollectionRefs, null);
            }
            return stageCollections;
        }
        #endregion

        #region Stages
        public async Task<StageDefinition> GetStageDefinition(string stageIdentifier)
        {
            if (stages.Count == 0)
            {
                await GetStageDefinitions();
            }
            return stages.FirstOrDefault(x => x.stageIdentifier == stageIdentifier);
        }

        public async Task<List<StageDefinition>> GetStageDefinitions()
        {
            if (stages.Count == 0 && stageRefs.Count > 0)
            {
                stages = await LoadRealData<StageDefinition>(stageRefs, null);
            }
            return stages;
        }

        public async UniTask<List<string>> LoadStageAsync(string stageIdentifier, LoadSceneMode loadMode)
        {
            LocalStageDefinition sd = (LocalStageDefinition) (await GetStageDefinition(stageIdentifier));

            List<string> loadedScenes = new List<string>(sd.sceneRefs.Length);
            for(int i = 0; i < sd.sceneRefs.Length; i++)
            {
                var result = await Addressables.LoadSceneAsync(sd.sceneRefs[i], loadMode).Task;
                await UniTask.NextFrame();
                if (result.Scene.isLoaded == false)
                {
                    loadedScenes.Add(null);
                    return loadedScenes;
                }
                loadedScenes.Add(result.Scene.name);
            }
            return loadedScenes;
        }
        #endregion
    }
}