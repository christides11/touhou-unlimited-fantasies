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

namespace TUF.Modding
{
    [CreateAssetMenu(fileName = "ModDefinition", menuName = "TAPI/ModDefinition")]
    public class ModDefinition : ScriptableObject
    {
        [TextArea] [SerializeField] private string description;
        [HideInInspector] public bool local = false;
        [SerializeField] private List<EntityDefinition> entities = new List<EntityDefinition>();
        [SerializeField] private List<GameModeDefinition> gamemodes = new List<GameModeDefinition>();
        [SerializeField] private List<StageDefinition> stages = new List<StageDefinition>();
        [SerializeField] private List<StageCollection> stageCollections = new List<StageCollection>();
        [SerializeField] private List<SoundDefinition> sounds = new List<SoundDefinition>();
        [SerializeField] private List<SoundDefinition> songs = new List<SoundDefinition>();
        [SerializeField] private List<UIDefinition> uis = new List<UIDefinition>();
        [SerializeField] private List<UIOverrideDefinition> uiOverrides = new List<UIOverrideDefinition>();

        #region Entities
        /// <summary>
        /// Gets all the characters in the mod.
        /// </summary>
        /// <returns>A list of all the characters in the mod.</returns>
        public virtual List<EntityDefinition> GetEntityDefinitions()
        {
            return entities;
        }

        /// <summary>
        /// Get a character by name. 
        /// </summary>
        /// <param name="characterName">The name of the character you want./</param>
        /// <returns>The EntityDefinition with the character name given. If it does not exist,
        /// returns null.</returns>
        public virtual EntityDefinition GetEntityDefinition(string entityName)
        {
            return entities.FirstOrDefault(x => x.entityName.ToLower() == entityName.ToLower());
        }
        #endregion

        #region Gamemodes
        /// <summary>
        /// Gets all the gamemodes in the mod.
        /// </summary>
        /// <returns>A list of all the gamemodes in the mod.</returns>
        public virtual List<GameModeDefinition> GetGamemodeDefinitions()
        {
            return gamemodes;
        }

        /// <summary>
        /// Gets a gamemode by name.
        /// </summary>
        /// <param name="gamemodeName"></param>
        /// <returns>The GameModeDefinition with the gamemode name given. If it does not exist,
        /// returns null.</returns>
        public virtual GameModeDefinition GetGamemodeDefinition(string gamemodeName)
        {
            return gamemodes.FirstOrDefault(x => x.gameModeName.ToLower() == gamemodeName.ToLower());
        }
        #endregion

        #region Stages
        /// <summary>
        /// Gets all the stages in the mod.
        /// </summary>
        /// <returns>A list of all the scenes in the mod.</returns>
        public virtual List<StageDefinition> GetStageDefinitions()
        {
            return stages;
        }

        /// <summary>
        /// Gets a stage by name.
        /// </summary>
        /// <param name="stageIdentifier">The name of the stage.</param>
        /// <returns>The SceneDefinition with the scene name given. If it does not exist,
        /// returns null.</returns>
        public virtual StageDefinition GetStageDefinition(string stageIdentifier)
        {
            return stages.FirstOrDefault(x => x.stageIdentifier.ToLower() == stageIdentifier.ToLower());
        }
        #endregion

        #region Stages Collections
        /// <summary>
        /// Gets all the stage collections in this mod.
        /// </summary>
        /// <returns>A list of all the stage collections in the mod.</returns>
        public virtual List<StageCollection> GetStageCollections()
        {
            return stageCollections;
        }

        /// <summary>
        /// Gets a stage collection by name.
        /// </summary>
        /// <param name="stageCollectionName">The name of the stage collection.</param>
        /// <returns>The StageCollection with the name given. If it does not exist,
        /// returns null.</returns>
        public virtual StageCollection GetStageCollection(string stageCollectionName)
        {
            return stageCollections.FirstOrDefault(x => x.collectionName.ToLower() == stageCollectionName.ToLower());
        }
        #endregion

        #region Sounds
        /// <summary>
        /// Gets all the sounds in this mod.
        /// </summary>
        /// <returns>A list of all the songs in the mod.</returns>
        public virtual List<SoundDefinition> GetSoundDefinitions()
        {
            return sounds;
        }

        /// <summary>
        /// Gets a sound by name.
        /// </summary>
        /// <param name="soundName">The name of the song.</param>
        /// <returns>The song with the name given. If it does not exist,
        /// returns null.</returns>
        public virtual SoundDefinition GetSoundDefinition(string soundName)
        {
            return sounds.FirstOrDefault(x => x.soundName.ToLower() == soundName.ToLower());
        }
        #endregion

        #region Songs
        /// <summary>
        /// Gets all the songs in this mod.
        /// </summary>
        /// <returns>A list of all the songs in the mod.</returns>
        public virtual List<SoundDefinition> GetSongDefinitions()
        {
            return songs;
        }

        /// <summary>
        /// Gets a song by name.
        /// </summary>
        /// <param name="songName">The name of the song.</param>
        /// <returns>The song with the name given. If it does not exist,
        /// returns null.</returns>
        public virtual SoundDefinition GetSongDefinition(string songName)
        {
            return songs.FirstOrDefault(x => x.soundName.ToLower() == songName.ToLower());
        }
        #endregion
    }
}