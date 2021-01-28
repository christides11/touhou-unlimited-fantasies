using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUF.Core;
using TUF.Entities.Shared;
using TUF.GameMode;
using TUF.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TUF.Modding
{
    public interface IModDefinition
    {
        bool LocalMod { get; }
        string Description { get; }

        // ENTITIES //
        Task<List<EntityDefinition>> GetEntityDefinitions();
        Task<EntityDefinition> GetEntityDefinition(string entityName);

        // GAMEMODES //
        Task<List<GameModeDefinition>> GetGamemodeDefinitions();
        Task<GameModeDefinition> GetGamemodeDefinition(string gamemodeID);

        // STAGES //
        Task<List<StageDefinition>> GetStageDefinitions();
        Task<StageDefinition> GetStageDefinition(string stageIdentifier);

        UniTask<List<string>> LoadStageAsync(string stageIdentifier, LoadSceneMode loadMode);

        // STAGE COLLECTIONS //
        Task<List<StageCollection>> GetStageCollections();
        Task<StageCollection> GetStageCollection(string stageCollectionName);

        // SOUNDS //
        List<SoundDefinition> GetSoundDefinitions();
        SoundDefinition GetSoundDefinition(string soundName);

        // SONGS //
        List<SoundDefinition> GetSongDefinitions();
        SoundDefinition GetSongDefinition(string songName);
    }
}