using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAPI.Core;
using TAPI.Entities.Shared;
using TAPI.GameMode;
using UnityEngine;

namespace TAPI.Modding
{
    [CreateAssetMenu(fileName = "ModDefinition", menuName = "TAPI/ModDefinition")]
    public class ModDefinition : ScriptableObject
    {
        [HideInInspector] public bool local = false;
        [SerializeField] private List<EntityDefinition> entities = new List<EntityDefinition>();
        [SerializeField] private List<GameModeDefinition> gamemodes = new List<GameModeDefinition>();
        [SerializeField] private List<StageDefinition> stages = new List<StageDefinition>();
        [SerializeField] private List<StageCollection> stageCollections = new List<StageCollection>();

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

        /// <summary>
        /// Gets all the scenes in the mod.
        /// </summary>
        /// <returns>A list of all the scenes in the mod.</returns>
        public virtual List<StageDefinition> GetStageDefinitions()
        {
            return stages;
        }

        /// <summary>
        /// Gets a scene by name.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns>The SceneDefinition with the scene name given. If it does not exist,
        /// returns null.</returns>
        public virtual StageDefinition GetStageDefinition(string stageName)
        {
            return stages.FirstOrDefault(x => x.stageName.ToLower() == stageName.ToLower());
        }

        public virtual List<StageCollection> GetStageCollections()
        {
            return stageCollections;
        }

        public virtual StageCollection GetStageCollection(string stageCollection)
        {
            return stageCollections.FirstOrDefault(x => x.collectionName.ToLower() == stageCollection.ToLower());
        }

        /// <summary>
        /// Loads the scene requested. If a mod, just return false.
        /// </summary>
        /// <returns>False if this is a umod mod.</returns>
        public virtual async Task<bool> LoadScene(string sceneName)
        {
            return false;
        }
    }
}